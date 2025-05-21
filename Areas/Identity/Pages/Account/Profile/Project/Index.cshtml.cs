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
    public Vacancy OwnedVacancy { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Challenge();

        OwnedVacancy = new Vacancy
        {
            Id = Guid.NewGuid(),
            Description = "Ну типа описание",
            DifficultyLevel = 3,
            Title = "Developer Oslik"
        };

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
}