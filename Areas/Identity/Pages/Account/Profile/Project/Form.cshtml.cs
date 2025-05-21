using Aeon_Web.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Aeon_Web.Areas.Project.Pages.Jobs;

public class FormModel : PageModel
{
    [BindProperty] 
    public Vacancy Vacancy { get; set; }

    public bool IsEdit { get; set; }

    [BindProperty] 
    public string SkillsRaw { get; set; } = "";

    public void OnGet(Guid? id)
    {
        if (id.HasValue)
        {
            IsEdit = true;

            var vacancy = LoadVacancyById(id.Value);
            if (vacancy is null)
            {
                //TODO добавить StatusMessage
            }

            SkillsRaw = string.Join(", ", Vacancy.VacancySkills);
        }
        else
        {
            IsEdit = false;
            Vacancy = new Vacancy
            {
                Id = Guid.NewGuid()
            };
        }
    }

    public IActionResult OnPost()
    {
        /*Job.SkillsRequired = SkillsRaw
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .ToList();*/

        if (!ModelState.IsValid)
            return Page();

        SaveJob(Vacancy); // сохранить или обновить

        return RedirectToPage("/Jobs");
    }

    private Vacancy? LoadVacancyById(Guid id)
    {
        // Заглушка
        return new Vacancy
        {
            Id = Guid.NewGuid(),
            Title = "Sample Job",
            DifficultyLevel = 5,
            Description = "Job description...",
            Contact = new ContactInfo { Email = "test@example.com" }
        };
    }

    private void SaveJob(Vacancy job)
    {
        // логика сохранения
    }
}