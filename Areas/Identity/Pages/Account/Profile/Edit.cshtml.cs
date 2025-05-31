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
    
    private Guid _resumeId;

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

    [TempData] 
    public string? StatusMessage { get; set; }

    /*[BindProperty] 
    public Resume Resume { get; set; }*/

    public bool IsHaveVisibleVacancy { get; set; }
    
    /*[BindProperty] 
    public string Skills { get; set; }*/
    
    [BindProperty]
    public InputModel Input { get; set; }

    public class InputModel
    {
        [BindProperty]
        public bool IsEdit { get; set; }
        
        public Guid ResumeId { get; set; }
        
        [Required]
        public string Title { get; set; }
        
        [Required]
        public string Summary { get; set; }
        
        [Required]
        [EmailAddress]
        [Display(Name = "example@mail.com")]
        public string Email { get; set; }
    
        [Phone]
        [Display(Name = "+12345678910")]
        public string? Phone { get; set; }
    
        public string? Website { get; set; }
        
        public string SkillsRaw { get; set; }
    }

    public async Task OnGetAsync(Guid? resumeId)
    {
        if (resumeId.HasValue)
        {
            var resume = await LoadResumeById(resumeId.Value);
            if (resume is null)
            {
                StatusMessage = "Ошибка - не удалось загрузить резюме";
                RedirectToPage("Index");
                return;
            }

            Load(resume);
            
            var user = await _userManager.GetUserAsync(User);
            if (user?.OwnedVacancyId != null)
            {
                var vacancy = await _unitOfWork.VacancyRepository.GetByIdAsync(user.OwnedVacancyId);
                if (vacancy is not null && vacancy.IsVisible)
                {
                    IsHaveVisibleVacancy = true;
                }
            }
        }
        else
        {
            Input = new InputModel
            {
                IsEdit = false,
                ResumeId = Guid.NewGuid()
            };
        }
    }

    public async Task<IActionResult> OnPostAsync()
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

        return RedirectToPage("Index");
    }

    public async Task<IActionResult> OnPostDeleteAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user is null)
        {
            StatusMessage = "Ошибка - не удалось удалить резюме";
            return RedirectToPage("Index");
        }
        user.ResumeId = null;

        var resume = GetResumeFromInput();
        
        await _resumeService.DeleteResumeAsync(resume.Id);

        StatusMessage = "Резюме удалено";

        return RedirectToPage("Index");
    }

    private async Task<Resume?> LoadResumeById(Guid id)
    {
        return await _unitOfWork.ResumeRepository.GetByIdAsync(id);
    }
    
    private void Load(Resume resume)
    {
        Input = new InputModel
        {
            IsEdit = true,
            ResumeId = resume.Id,
            Title = resume.Title,
            Summary = resume.Summary,
            Email = resume.Contact.Email,
            Phone = resume.Contact.Phone,
            Website = resume.Contact.Website,
            SkillsRaw = string.Join(", ", resume.ResumeSkills.Select(s => s.Skill?.Name))
        };
    }
    
    private async Task CreateResume()
    {
        var resume = GetResumeFromInput();
        
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
    
    private async Task EditResume()
    {
        var existingResume = await _unitOfWork.ResumeRepository.GetByIdAsync(Input.ResumeId);
        if (existingResume is null)
        {
            StatusMessage = "Ошибка! Не удалось обновить резюме";
            Redirect("./Index");
            return;
        }
        
        /*var skills = string.Join(", ", resume.ResumeSkills.Select(s => s.Skill?.Name));
        
        var newSkills = Input.SkillsRaw.Split(',');*/
        
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
            .Where(rs => toRemove.Contains(rs.Skill?.Name?.Trim().ToLower()))
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
        existingResume.Contact.Email = Input.Email;
        existingResume.Contact.Phone = Input.Phone;
        existingResume.Contact.Website = Input.Website;
        await _unitOfWork.ResumeRepository.UpdateAsync(existingResume);
        StatusMessage = "Резюме обновлено";
    }

    private Resume GetResumeFromInput()
    {
        return new Resume
        {
            Id = Input.ResumeId,
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
}