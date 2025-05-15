using Aeon_Web.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Aeon_Web.Areas.Identity.Pages.Account.Profile;

public class Edit : PageModel
{
    [BindProperty]
    public Resume Resume { get; set; }
    
    public string Skills { get; set; }

    public void OnGet()
    {
        // В реальном проекте здесь ты подгрузишь данные из БД
        Resume = new Resume
        {
            FullName = "Иван Иванов",
            Title = "Backend Developer",
            Summary = "Опытный разработчик .NET с фокусом на web API",
            Skills = new List<string> { "C#", ".NET", "SQL" },
            Contact = new ContactInfo
            {
                Email = "ivan@example.com",
                Phone = "+7 999 123-45-67",
                Website = "https://ivan.dev"
            }
        };
        
        Skills = string.Join(", ", Resume.Skills);
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        // Здесь сохраняешь данные, например, в БД

        return RedirectToPage("Success"); // или куда нужно
    }
}