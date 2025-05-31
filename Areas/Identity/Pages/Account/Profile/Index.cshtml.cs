using Aeon_Web.Data.Repository.Abstractions;
using Aeon_Web.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Aeon_Web.Areas.Identity.Pages.Account.Profile;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUnitOfWork _unitOfWork;
    
    public IndexModel(ILogger<IndexModel> logger,
        UserManager<ApplicationUser> userManager,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _userManager = userManager;
        _unitOfWork = unitOfWork;
    }

    [TempData]
    public string StatusMessage { get; set; }

    public Resume? Resume { get; set; }
    
    public IEnumerable<Skill> Skills { get; set; }
    
    public async Task OnGet()
    {
        var user = await LoadUser();

        Resume = user.Resume;

        var skillIds = user.Resume?.ResumeSkills.Select(rs => rs.SkillId).ToList();

        var skills = skillIds is not null && skillIds.Count != 0
            ? await _unitOfWork.SkillRepository.GetAsync(s => skillIds.Contains(s.Id))
            : new List<Skill>();

        Skills = skills;
        
        /*Resume = new Resume
        {
            Id = Guid.NewGuid(),
            Title = "Backend Developer",
            Summary = "Опытный разработчик .NET с фокусом на web API",
            Contact = new ContactInfo
            {
                Email = "mail@example.com",
                Phone = "+7 999 123-45-67",
                Website = "example.com"
            }
        };

        Skills = new[]
        {
            new Skill()
            {
                Id = Guid.NewGuid(),
                Name = "C#"
            },
            new Skill()
            {
                Id = Guid.NewGuid(),
                Name = "Git"
            },
            new Skill()
            {
                Id = Guid.NewGuid(),
                Name = "Asp"
            }
        };*/

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