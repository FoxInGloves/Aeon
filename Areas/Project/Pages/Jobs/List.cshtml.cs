using Aeon_Web.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Aeon_Web.Areas.Project.Pages.Jobs;

public class ListModel : PageModel
{
    public List<Vacancy>? Vacancies { get; set; } = new();
    
    [BindProperty(SupportsGet = true)]
    public string? SearchQuery { get; set; }
    
    [BindProperty(SupportsGet = true)]
    public int? MaxDifficulty { get; set; }

    public void OnGet()
    {
        Vacancies = new List<Vacancy>();

        for (var i = 0; i < 10; i++)
        {
            Vacancies.AddRange([
                new Vacancy
                {
                    Id = Guid.NewGuid(),
                    Title = "Backend Developer",
                    DifficultyLevel = 1,
                    PostedDate = DateTime.UtcNow.AddDays(-2),
                },
                new Vacancy
                {
                    Id = Guid.NewGuid(),
                    Title = "Junior Frontend Developer",
                    DifficultyLevel = 5,
                    PostedDate = DateTime.UtcNow.AddDays(-1)
                }
            ]);
        }
    }
}