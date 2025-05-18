using Aeon_Web.Models.Entities;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Aeon_Web.Areas.Project.Pages.Jobs;

public class Swipe : PageModel
{
    public List<Vacancy> Vacancies { get; set; } = new();

    public void OnGet()
    {
        // Пример заполнения — замени на реальные данные из БД
        Vacancies = new List<Vacancy>();

        for (var i = 0; i < 10; i++)
        {
            Vacancies.AddRange([
                new Vacancy
                {
                    Id = Guid.NewGuid(),
                    Title = "Backend Developer",
                    DifficultyLevel = 4,
                    Description = "We're looking for an experienced .NET developer to join our team.",
                    PostedDate = DateTime.UtcNow.AddDays(-2)
                },
                new Vacancy
                {
                    Id = Guid.NewGuid(),
                    Title = "Junior Frontend Developer",
                    DifficultyLevel = 7,
                    PostedDate = DateTime.UtcNow.AddDays(-1)
                }
            ]);
        }
    }
}