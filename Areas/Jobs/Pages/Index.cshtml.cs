using Aeon_Web.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Aeon_Web.Areas.Jobs.Pages;

public class Index : PageModel
{
    public List<JobVacancy>? Vacancies { get; set; }
    
    public void OnGet()
    {
        
    }
}