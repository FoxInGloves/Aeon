using Aeon_Web.Models.Entities;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Aeon_Web.Areas.Project.Pages.Jobs;

public class Details : PageModel
{
    public Vacancy Job { get; set; }

    public void OnGet(Guid id)
    {
        // Замените на реальное получение данных из БД
        Job = new Vacancy
        {
            Id = id,
            Title = "Senior .NET Developer",
            Description = "We're looking for an experienced .NET developer to join our team.",
            DifficultyLevel = 1,
            SkillsRequired = new List<string> { "C#", "ASP.NET Core", "Azure", "SQL" },
            PostedDate = DateTime.UtcNow,
            Contact = new ContactInfo
            {
                Email = "hr@techcorp.com",
                Phone = "+123456789"
            }
        };
    }
}