using Aeon_Web.Data.Repository.Abstractions;
using Aeon_Web.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Aeon_Web.Areas.Project.Pages.Job;

public class ListModel : PageModel
{
    private readonly ILogger<ListModel> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<ApplicationUser> _userManager;

    public ListModel(
        ILogger<ListModel> logger,
        IUnitOfWork unitOfWork,
        UserManager<ApplicationUser> userManager)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _userManager = userManager;
    }

    public List<Vacancy> Vacancies { get; set; } = [];

    [BindProperty(SupportsGet = true)]
    public string? SearchQuery { get; set; }

    [BindProperty(SupportsGet = true)]
    public int? MaxDifficulty { get; set; }

    public async Task OnGetAsync()
    {
        var vacancies = await GetFilteredVacanciesAsync();
        var currentUser = await _userManager.GetUserAsync(User);

        if (currentUser?.OwnedVacancy is { IsVisible: true })
        {
            vacancies = vacancies
                .Where(v => v.Id != currentUser.OwnedVacancyId)
                .ToList();
        }

        Vacancies = vacancies;
    }

    private async Task<List<Vacancy>> GetFilteredVacanciesAsync()
    {
        var query = await _unitOfWork.VacancyRepository.GetAsync(v => v.IsVisible);

        if (!string.IsNullOrWhiteSpace(SearchQuery))
        {
            query = query.Where(v =>
                v.Title.Contains(SearchQuery!) ||
                v.Description.Contains(SearchQuery!) ||
                v.VacancySkills.Any(vs => vs.Skill.Name.Contains(SearchQuery!)));
        }

        if (MaxDifficulty.HasValue)
        {
            query = query.Where(v => v.DifficultyLevel <= MaxDifficulty.Value);
        }

        return query.ToList();
    }
}