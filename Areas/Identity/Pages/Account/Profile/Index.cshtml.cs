using Aeon_Web.Models.Entities;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Aeon_Web.Areas.Identity.Pages.Account.Profile;

public class IndexModel : PageModel
{
    public Resume? Resume { get; set; }
    
    public void OnGet()
    {
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
        
        LoadUser();
    }

    private void LoadUser()
    {
        
    }
}