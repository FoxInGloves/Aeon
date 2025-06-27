using Aeon_Web.Data.Repository.Abstractions;
using Aeon_Web.Models.Entities;
using Aeon_Web.Services.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Aeon_Web.Areas.Identity.Pages.Account.Profile.Project;

public class FormModel : PageModel
{
    private readonly ILogger<FormModel> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<ApplicationUser> _userManager;

    public FormModel(
        ILogger<FormModel> logger,
        IUnitOfWork unitOfWork,
        UserManager<ApplicationUser> userManager)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _userManager = userManager;
    }
    
    [TempData] 
    public string? StatusMessage { get; set; }

    [BindProperty]
    public bool IsHaveVisibleResume { get; set; }
    
    [BindProperty] public InputModel Input { get; set; }
    
    public class InputModel
    {
        public bool IsEdit { get; set; }
        
        public Guid VacancyId { get; set; }
        
        public bool IsVisible { get; set; }
    
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public byte DifficultyLevel { get; set; }
        
        public string Email { get; set; } = string.Empty;
    
        public string? Phone { get; set; }
    
        public string? Website { get; set; }

        public string SkillsRaw { get; set; } = "";
    }

    public async Task OnGet(Guid? vacancyId)
    {
        if (vacancyId.HasValue)
        {
            var vacancy = _unitOfWork.VacancyRepository.GetByIdAsync(vacancyId.Value).Result;
            if (vacancy is null)
            {
                StatusMessage = "Ошибка! Не удалось загрузить проект";
                RedirectToPage("Index");
                return;
            }
            
            LoadVacancy(vacancy);

            var user = await _userManager.GetUserAsync(User);
            if (user?.ResumeId != null)
            {
                var resume = await _unitOfWork.ResumeRepository.GetByIdAsync(user.ResumeId);
                if (resume is not null && resume.IsVisible)
                {
                    IsHaveVisibleResume = true;
                }
            }
        }
        else
        {
            Input = new InputModel
            {
                VacancyId = Guid.NewGuid()
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
            await EditVacancy();
        }
        else
        {
            await CreateVacancy();
        }

        await _unitOfWork.SaveChangesAsync();

        return RedirectToPage("./Index");
    }

    private void LoadVacancy(Vacancy vacancy)
    {
        var skillsRaw = vacancy.VacancySkills.Select(vs => vs.Skill?.Name).ToList();

        Input = new InputModel
        {
            IsEdit = true,
            VacancyId = vacancy.Id,
            IsVisible = vacancy.IsVisible,
            Title = vacancy.Title,
            Description = vacancy.Description,
            DifficultyLevel = vacancy.DifficultyLevel,
            Email = vacancy.Contact.Email,
            Phone = vacancy.Contact.Phone,
            Website = vacancy.Contact.Website,
            SkillsRaw = string.Join(", ", skillsRaw)
        };
        
    }
    
    private async Task<ApplicationUser> GetUserAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user is null)
            throw new ArgumentNullException(User.ToString());

        return user;
    }

    private async Task EditVacancy()
    {
         var existingVacancy = await _unitOfWork.VacancyRepository.GetByIdAsync(Input.VacancyId);
        if (existingVacancy is null)
        {
            StatusMessage = "Ошибка! Не удалось обновить проект";
            Redirect("./Index");
            return;
        }

        if (IsHaveVisibleResume && Input.IsVisible)
        {
            await RemoveResumeFromPublicationAsync();
        }
        
        var existingSkills = existingVacancy.VacancySkills
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

        var resumeSkillsToDelete = existingVacancy.VacancySkills
            .Where(rs => toRemove.Contains(rs.Skill?.Name.Trim().ToLower()))
            .ToList();
        
        foreach (var rs in resumeSkillsToDelete)
        {
            _unitOfWork.VacancySkillRepository.Delete(rs);

            var skill = rs.Skill;
            if (skill is null)
                continue;

            var isUsedElsewhere = skill.VacancySkills.Any(rsx => rsx.VacancyId != existingVacancy.Id)
                                  || skill.VacancySkills.Count != 0;

            if (!isUsedElsewhere)
            {
                _unitOfWork.SkillRepository.Delete(skill);
            }
        }
        
        foreach (var skillName in toAdd.OfType<string>())
        {
            var skillForResume = await _unitOfWork.SkillRepository.GetOrCreateSkillAsync(skillName);
            await _unitOfWork.VacancySkillRepository.CreateAsync(
                new VacancySkill { VacancyId = existingVacancy.Id, SkillId = skillForResume.Id });
        }

        existingVacancy.Title = Input.Title;
        existingVacancy.Description = Input.Description;
        existingVacancy.IsVisible = Input.IsVisible;
        existingVacancy.DifficultyLevel = Input.DifficultyLevel;
        existingVacancy.Contact.Email = Input.Email;
        existingVacancy.Contact.Phone = Input.Phone;
        existingVacancy.Contact.Website = Input.Website;
        await _unitOfWork.VacancyRepository.UpdateAsync(existingVacancy);
        StatusMessage = "Проект обновлен";
    }
    
    private async Task CreateVacancy()
    {
        var vacancy = GetVacancyFromInput();
        
        if (IsHaveVisibleResume && Input.IsVisible)
        {
            await RemoveResumeFromPublicationAsync();
        }
        
        foreach (var skillName in Input.SkillsRaw.Split(',', StringSplitOptions.RemoveEmptyEntries))
        {
            var skillForVacancy = await _unitOfWork.SkillRepository.GetOrCreateSkillAsync(skillName);
            await _unitOfWork.VacancySkillRepository.CreateAsync(
                new VacancySkill { VacancyId = vacancy.Id, SkillId = skillForVacancy.Id });
        }
        
        var user = await _userManager.GetUserAsync(User);
        if (user is null)
        {
            StatusMessage = "Ошибка - не удалось создать проект";
            return;
        }

        await _unitOfWork.VacancyRepository.CreateAsync(vacancy);
        user.OwnedVacancyId = vacancy.Id;
        await _unitOfWork.UserRepository.UpdateAsync(user);
        StatusMessage = "Проект создан";
    }

    private Vacancy GetVacancyFromInput()
    {
        return new Vacancy
        {
            Id = Input.VacancyId,
            IsVisible = true,
            Title = Input.Title,
            Description = Input.Description,
            DifficultyLevel = Input.DifficultyLevel,
            Contact = new ContactInfo
            {
                Email = Input.Email,
                Phone = Input.Phone,
                Website = Input.Website
            }
        };
    }
    
    private async Task RemoveResumeFromPublicationAsync()
    {
        var user = await GetUserAsync();
        if (user.ResumeId is null)
        {
            return;
        }
        
        var resume = await _unitOfWork.ResumeRepository.GetByIdAsync(user.ResumeId);
        if (resume is null)
        {
            throw new NullReferenceException(user.ResumeId.ToString());
        }

        if (resume.IsVisible)
        {
            resume.IsVisible = false;
        }
    }
}