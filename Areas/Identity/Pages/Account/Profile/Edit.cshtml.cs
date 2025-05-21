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

        Resume = new Resume
        {
            Id = Guid.NewGuid()
        };
        Skills = string.Empty;
    }

    [TempData] 
    public string? StatusMessage { get; set; }

    [BindProperty] 
    public Resume Resume { get; set; }

    public bool IsEdit { get; set; }

    [BindProperty] 
    public string Skills { get; set; }

    public async Task OnGetAsync(Guid? resumeId)
    {
        if (resumeId.HasValue)
        {
            IsEdit = true;
            
            var resume = await LoadResumeById(resumeId.Value);
            if (resume is null)
            {
                StatusMessage = "Ошибка - не удалось загрузить резюме";
                RedirectToPage("Index");
            }

            Skills = string.Join(", ", Resume.ResumeSkills.Select(s => s.Skill?.Name));
        }
        else
        {
            IsEdit = false;

            /*Resume = new Resume
            {
                Id = Guid.NewGuid()
            };*/

            _resumeId = Guid.NewGuid();
        }

        /*Resume = new Resume
        {
            Id = Guid.NewGuid(),
            FullName = "Иван Иванов",
            Title = "Backend Developer",
            Summary = "Опытный разработчик .NET с фокусом на web API",
            Contact = new ContactInfo
            {
                Email = "ivan@example.com",
                Phone = "+7 999 123-45-67",
                Website = "https://ivan.dev"
            }
        };*/
        
        Console.WriteLine(_resumeId);
    }

    public async Task<IActionResult> OnPostAsync()
    {
        Console.WriteLine(_resumeId);
        if (!ModelState.IsValid)
        {
            StatusMessage = "Ошибка - не все поля заполнены";
            return Page();
        }

        var skillsForResume = new List<Skill>();
        foreach (var skillName in Skills.Split(','))
        {
            var skillForResume = await _unitOfWork.SkillRepository.GetOrCreateSkillAsync(skillName);
            await _unitOfWork.ResumeSkillRepository.CreateAsync(
                new ResumeSkill { ResumeId = _resumeId, SkillId = skillForResume.Id });
            skillsForResume.Add(skillForResume);
        }

        //Resume.Skills = skillsForResume;

        if (IsEdit)
        {
            await _unitOfWork.ResumeRepository.UpdateAsync(Resume);
            StatusMessage = "Резюме обновлено";
        }
        else
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null)
            {
                StatusMessage = "Ошибка - не удалось создать резюме";
                return RedirectToPage("Index");
            }

            await _unitOfWork.ResumeRepository.CreateAsync(Resume);
            user.ResumeId = Resume.Id;
            await _unitOfWork.UserRepository.UpdateAsync(user);
            StatusMessage = "Резюме создано";
        }

        await _unitOfWork.SaveChangesAsync();

        // Здесь сохраняешь данные, например, в БД

        return RedirectToPage("Index"); // или куда нужно
    }

    public async Task<IActionResult> OnPostDeleteAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user is null)
        {
            StatusMessage = "Ошибка - не удалось удалить резюме";
            return RedirectToPage("Index");
        }
        //user.ResumeId = null;

        await _resumeService.DeleteResumeAsync(Resume.Id);

        StatusMessage = "Резюме удалено";

        return RedirectToPage("Index");
    }

    private async Task<Resume?> LoadResumeById(Guid id)
    {
        return await _unitOfWork.ResumeRepository.GetByIdAsync(id);
    }
}