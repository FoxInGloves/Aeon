using Aeon_Web.Data.Repository.Abstractions;
using Aeon_Web.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Aeon_Web.Areas.Identity.Pages.Account.Profile.Project;

public class IndexModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUnitOfWork _unitOfWork;

    public IndexModel(UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _unitOfWork = unitOfWork;
    }

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
        /*OwnedVacancy = new Vacancy
        {
            Id = Guid.NewGuid(),
            Description = "Лучший сервис",
            DifficultyLevel = 3,
            Title = "Aeon",
        };

        Skills = new[]
        {
            new Skill()
            {
                Id = Guid.NewGuid(),
                Name = "C#"
            },
            new Skill()
            {
                Id = Guid.NewGuid(),
                Name = "Git"
            },
            new Skill()
            {
                Id = Guid.NewGuid(),
                Name = "Asp"
            }
        };*/
        
        /*if (user.OwnedVacancyId == null)
        {
            return RedirectToPage("/Vacancy/Create"); // Или предложение создать
        }*/

        /*OwnedVacancy = await _db.Vacancies
            .Include(v => v.VacancySkills)
            .ThenInclude(vs => vs.Skill)
            .FirstOrDefaultAsync(v => v.Id == user.OwnedVacancyId);*/

        return Page();
    }

    public async Task OnPostDeleteAsync()
    {
        //TODO написать удаление проекта
    }
}