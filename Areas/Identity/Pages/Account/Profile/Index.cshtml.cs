using Aeon_Web.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Aeon_Web.Areas.Identity.Pages.Account.Profile;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly UserManager<ApplicationUser> _userManager;
    
    public IndexModel(ILogger<IndexModel> logger, UserManager<ApplicationUser> userManager)
    {
        _logger = logger;
        _userManager = userManager;
    }

    [TempData]
    public string StatusMessage { get; set; }

    public Resume? Resume { get; set; }
    
    public IEnumerable<Skill> Skills { get; set; }
    
    public async Task OnGet()
    {
        var user = await LoadUser();

        Resume = user.Resume;
        
        /*Resume = new Resume
        {
            Id = Guid.NewGuid(),
            FullName = "Иван Иванов",
            Title = "Backend Developer",
            Summary = "Опытный разработчик .NET с фокусом на web API",
            Contact = new ContactInfo
            {
                Email = "ivan@example.com",
                Phone = "+7 999 123-45-67",
                Website = "https://ivan.dev"
            }
        };
        */ 
    }

    private async Task<ApplicationUser> LoadUser()
    {
        var user = await _userManager.GetUserAsync(User);

        if (user is null)
        {
            //TODO что то делать, если пользователь не найден
            _logger.LogError("User not found");
        }
        
        return user;
    }
}