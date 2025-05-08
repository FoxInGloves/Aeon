using Aeon_Web.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Aeon_Web.Areas.Jobs.Pages;

public class JobList : PageModel
{
    public List<JobVacancy>? Vacancies { get; set; } = new();

    public void OnGet()
    {
        // Здесь будет получение из БД. Пока что — заглушка:
        Vacancies = new List<JobVacancy>
        {
            new JobVacancy
            {
                Id = Guid.NewGuid(),
                Title = "Backend Developer",
                CompanyName = "TechCorp",
                Location = "Москва",
                ExperienceLevel = ExperienceLevel.Mid,
                PostedDate = DateTime.UtcNow.AddDays(-2)
            },
            new JobVacancy
            {
                Id = Guid.NewGuid(),
                Title = "Junior Frontend Developer",
                CompanyName = "WebStart",
                Location = "Удалённо",
                ExperienceLevel = ExperienceLevel.Junior,
                PostedDate = DateTime.UtcNow.AddDays(-1)
            }
        };
    }
}