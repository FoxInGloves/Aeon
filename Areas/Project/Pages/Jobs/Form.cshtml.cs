using Aeon_Web.Models.Entities;
using Aeon_Web.Models.Entities.Resume;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Aeon_Web.Areas.Project.Pages.Jobs;

public class FormModel : PageModel
{
    [BindProperty]
    public JobVacancy Job { get; set; } = new();

    [BindProperty]
    public string SkillsRaw { get; set; } = "";

    public void OnGet(Guid? id)
    {
        if (id.HasValue)
        {
            // Здесь загрузи вакансию из базы
            Job = LoadJobById(id.Value);
            SkillsRaw = string.Join(", ", Job.SkillsRequired);
        }
    }

    public IActionResult OnPost()
    {
        Job.SkillsRequired = SkillsRaw
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .ToList();

        if (!ModelState.IsValid)
            return Page();

        SaveJob(Job); // сохранить или обновить

        return RedirectToPage("/Jobs");
    }

    private JobVacancy LoadJobById(Guid id)
    {
        // Заглушка
        return new JobVacancy
        {
            Id = id,
            Title = "Sample Job",
            DifficultyLevel = 5,
            Description = "Job description...",
            SkillsRequired = new List<string> { "C#", "Razor", "HTML" },
            Contact = new ContactInfo { Email = "test@example.com" }
        };
    }

    private void SaveJob(JobVacancy job)
    {
        // логика сохранения
    }
}