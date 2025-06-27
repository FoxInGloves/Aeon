using Aeon_Web.Data.Repository.Abstractions;
using Aeon_Web.Models.Entities;
using Aeon_Web.Services.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Aeon_Web.Areas.Identity.Pages.Account.Profile.Project;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IVacancyService _vacancyService;

    public IndexModel(
        ILogger<IndexModel> logger,
        UserManager<ApplicationUser> userManager,
        IUnitOfWork unitOfWork,
        IVacancyService vacancyService)
    {
        _logger =  logger;
        _userManager = userManager;
        _unitOfWork = unitOfWork;
        _vacancyService = vacancyService;
    }
    
    [TempData] public string? StatusMessage { get; set; }

    [BindProperty] 
    public Vacancy? OwnedVacancy { get; set; }
    
    public IEnumerable<Skill> Skills { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Challenge();
        
        OwnedVacancy = user.OwnedVacancy;

        var skillIds = user.OwnedVacancy?.VacancySkills.Select(vs => vs.SkillId).ToList();

        var skills = skillIds is not null && skillIds.Count != 0
            ? await _unitOfWork.SkillRepository.GetAsync(s => skillIds.Contains(s.Id))
            : new List<Skill>();

        Skills = skills;

        return Page();
    }

    public async Task<IActionResult> OnPostDeleteAsync()
    {
        try
        {
            var user = await GetUserAsync();
            var vacancyId = user.OwnedVacancyId;
            user.OwnedVacancyId = null;
            
            await _vacancyService.DeleteVacancyAsync(vacancyId, user.Id);

            StatusMessage = "Проект удален";
        }
        catch (Exception e)
        {
            _logger.LogError("Cannot delete vacancy. {EMessage}", e.Message);
            StatusMessage = "Ошибка - не удалось удалить проект";
        }

        return RedirectToPage("Index");
    }
    
    private async Task<ApplicationUser> GetUserAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user is null)
            throw new ArgumentNullException(User.ToString());

        return user;
    }
}