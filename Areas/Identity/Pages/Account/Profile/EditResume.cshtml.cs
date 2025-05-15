using Aeon_Web.Models.Entities;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Aeon_Web.Areas.Identity.Pages.Account.Profile;

public class EditResume : PageModel
{
    public string? Title { get; set; }
    
    public string? Description { get; set; }

    public string? Name { get; set; }
    
    public string? Role { get; set; }
    
    public string? Email { get; set; }
    
    public void OnGet(Resume? resume)
    {
        /*if (resume is null) return;
        Description = resume.Description;
        Name = resume.Name;
        Role = resume.Role;
        Email = resume.Email;*/
    }

    public void OnPostSaveResume()
    {
        
    }
}