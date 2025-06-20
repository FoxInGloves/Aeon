using System.ComponentModel.DataAnnotations;
using Aeon_Web.Data.Repository.Abstractions;
using Aeon_Web.Models.Entities;
using Aeon_Web.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Aeon_Web.Areas.Identity.Pages.Account.Profile;

public class EditModel : PageModel
{
    private readonly ILogger<EditModel> _logger;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ResumeService _resumeService;

    public EditModel(
        ILogger<EditModel> logger,
        UserManager<ApplicationUser> userManager,
        IUnitOfWork unitOfWork,
        ResumeService resumeService)
    {
        _logger = logger;
        _userManager = userManager;
        _unitOfWork = unitOfWork;
        _resumeService = resumeService;
    }

    [TempData] public string? StatusMessage { get; set; }

    public bool IsHaveVisibleVacancy { get; set; }

    [BindProperty] public InputModel Input { get; set; }

    public class InputModel
    {
        public bool IsEdit { get; set; }

        public Guid ResumeId { get; set; }

        [Required] public string FullName { get; set; }

        [Required] public string Title { get; set; }

        [Required] public string Summary { get; set; }

        public bool IsVisible { get; set; }

        [Required] [EmailAddress] public string Email { get; set; }

        [Phone] public string? Phone { get; set; }

        public string? Website { get; set; }

        public string SkillsRaw { get; set; }
    }

    public async Task<IActionResult> OnGetAsync(Guid? resumeId)
    {
        try
        {
            if (!resumeId.HasValue)
            {
                Input = new InputModel
                {
                    ResumeId = Guid.NewGuid(),
                    IsVisible = true
                };
                return Page();
            }

            var resume = await LoadResumeById(resumeId.Value);
            if (resume is null)
            {
                StatusMessage = "Ошибка! Не удалось загрузить резюме";
                return RedirectToPage("Index");
            }

            LoadResume(resume);

            var user = await GetUserAsync();
            if (user.OwnedVacancyId is not { } vacancyId)
                return Page();

            var vacancy = await _unitOfWork.VacancyRepository.GetByIdAsync(vacancyId);
            IsHaveVisibleVacancy = vacancy?.IsVisible == true;
            return Page();
        }
        catch (Exception e)
        {
            _logger.LogError("{error}", e.Message);
            StatusMessage = "Ошибка - не удалось загрузить резюме";
            
            return RedirectToPage("Index");
        }
    }


    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            if (!ModelState.IsValid)
            {
                StatusMessage = "Ошибка - не все поля заполнены";
                return Page();
            }

            if (Input.IsEdit)
            {
                await EditResume();
            }
            else
            {
                await CreateResume();
            }

            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception e)
        {
            _logger.LogError("{error}", e.Message);
            StatusMessage = Input.IsEdit ? 
                "Ошибка - не удалось отредактировать резюме" : "Ошибка - не удалось создать резюме";
        }

        return RedirectToPage("Index");
    }

    public async Task<IActionResult> OnPostDeleteAsync()
    {
        try
        {
            var user = await GetUserAsync();
            var resumeId = user.ResumeId;
            user.ResumeId = null;
            
            await _resumeService.DeleteResumeAsync(resumeId);

            StatusMessage = "Резюме удалено";
        }
        catch (Exception e)
        {
            _logger.LogError("Cannot delete resume. {EMessage}", e.Message);
            StatusMessage = "Ошибка - не удалось удалить резюме";
        }

        return RedirectToPage("Index");
    }

    private async Task<Resume?> LoadResumeById(Guid id)
    {
        return await _unitOfWork.ResumeRepository.GetByIdAsync(id);
    }

    private async Task<ApplicationUser> GetUserAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user is null)
            throw new ArgumentNullException(User.ToString());

        return user;
    }

    private void LoadResume(Resume resume)
    {
        Input = new InputModel
        {
            IsEdit = true,
            ResumeId = resume.Id,
            FullName = resume.FullName,
            Title = resume.Title,
            Summary = resume.Summary,
            IsVisible = resume.IsVisible,
            Email = resume.Contact.Email,
            Phone = resume.Contact.Phone,
            Website = resume.Contact.Website,
            SkillsRaw = string.Join(", ", resume.ResumeSkills.Select(s => s.Skill?.Name))
        };
    }

    private async Task EditResume()
    {
        var existingResume = await _unitOfWork.ResumeRepository.GetByIdAsync(Input.ResumeId);
        if (existingResume is null)
        {
            StatusMessage = "Ошибка! Не удалось обновить резюме";
            RedirectToPage("/Index");
            return;
        }
        
        if (IsHaveVisibleVacancy && Input.IsVisible)
        {
            await RemoveVacancyFromPublicationAsync();
        }

        var existingSkills = existingResume.ResumeSkills
            .Select(s => s.Skill?.Name.Trim())
            .Where(name => !string.IsNullOrWhiteSpace(name))
            .ToHashSet();

        var incomingSkills = Input.SkillsRaw
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(s => s.Trim())
            .Where(name => !string.IsNullOrWhiteSpace(name))
            .ToHashSet();

        var toAdd = incomingSkills.Except(existingSkills).ToList();
        var toRemove = existingSkills.Except(incomingSkills).ToList();

        var resumeSkillsToDelete = existingResume.ResumeSkills
            .Where(rs => toRemove.Contains(rs.Skill?.Name.Trim().ToLower()))
            .ToList();

        foreach (var rs in resumeSkillsToDelete)
        {
            _unitOfWork.ResumeSkillRepository.Delete(rs);

            var skill = rs.Skill;
            if (skill is null)
                continue;

            var isUsedElsewhere = skill.ResumeSkills.Any(rsx => rsx.ResumeId != existingResume.Id)
                                  || skill.VacancySkills.Count != 0;

            if (!isUsedElsewhere)
            {
                _unitOfWork.SkillRepository.Delete(skill);
            }
        }

        foreach (var skillName in toAdd.OfType<string>())
        {
            var skillForResume = await _unitOfWork.SkillRepository.GetOrCreateSkillAsync(skillName);
            await _unitOfWork.ResumeSkillRepository.CreateAsync(
                new ResumeSkill { ResumeId = existingResume.Id, SkillId = skillForResume.Id });
        }

        existingResume.Title = Input.Title;
        existingResume.Summary = Input.Summary;
        existingResume.IsVisible = Input.IsVisible;
        existingResume.Contact.Email = Input.Email;
        existingResume.Contact.Phone = Input.Phone;
        existingResume.Contact.Website = Input.Website;
        await _unitOfWork.ResumeRepository.UpdateAsync(existingResume);
        StatusMessage = "Резюме обновлено";
    }

    private async Task CreateResume()
    {
        var resume = GetResumeFromInput();

        if (IsHaveVisibleVacancy && resume.IsVisible)
        {
            await RemoveVacancyFromPublicationAsync();
        }

        foreach (var skillName in Input.SkillsRaw.Split(',', StringSplitOptions.RemoveEmptyEntries))
        {
            var skillForResume = await _unitOfWork.SkillRepository.GetOrCreateSkillAsync(skillName);
            await _unitOfWork.ResumeSkillRepository.CreateAsync(
                new ResumeSkill { ResumeId = resume.Id, SkillId = skillForResume.Id });
        }

        var user = await _userManager.GetUserAsync(User);
        if (user is null)
        {
            StatusMessage = "Ошибка - не удалось создать резюме";
            return;
        }

        await _unitOfWork.ResumeRepository.CreateAsync(resume);
        user.ResumeId = resume.Id;
        await _unitOfWork.UserRepository.UpdateAsync(user);
        StatusMessage = "Резюме создано";
    }

    private Resume GetResumeFromInput()
    {
        return new Resume
        {
            Id = Input.ResumeId,
            IsVisible = Input.IsVisible,
            FullName = Input.FullName,
            Title = Input.Title,
            Summary = Input.Summary,
            Contact = new ContactInfo
            {
                Email = Input.Email,
                Phone = Input.Phone,
                Website = Input.Website
            }
        };
    }

    private async Task RemoveVacancyFromPublicationAsync()
    {
        var user = await GetUserAsync();
        if (user.OwnedVacancyId is null)
        {
            return;
        }
        
        var vacancy = await _unitOfWork.VacancyRepository.GetByIdAsync(user.OwnedVacancyId);
        if (vacancy is null)
        {
            throw new NullReferenceException(user.OwnedVacancyId.ToString());
        }

        if (vacancy.IsVisible)
        {
            vacancy.IsVisible = false;
        }
    }
}