using Aeon_Web.Models.Entities;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Aeon_Web.Areas.Project.Pages.Jobs;

public class ListModel : PageModel
{
    public List<JobVacancy>? Vacancies { get; set; } = new();

    public void OnGet()
    {
        Vacancies = new List<JobVacancy>();

        for (var i = 0; i < 10; i++)
        {
            Vacancies.AddRange([
                new JobVacancy
                {
                    Id = Guid.NewGuid(),
                    Title = "Backend Developer",
                    DifficultyLevel = 1,
                    PostedDate = DateTime.UtcNow.AddDays(-2),
                    SkillsRequired = ["c#", "c", "java", "RabbitMQ"]
                },
                new JobVacancy
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