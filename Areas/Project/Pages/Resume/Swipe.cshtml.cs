using Aeon_Web.Data.Repository.Abstractions;
using Aeon_Web.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Aeon_Web.Areas.Project.Pages.Resume;

public class SwipeModel : PageModel
{
    private readonly ILogger<SwipeModel> _logger;
    private readonly IUnitOfWork  _unitOfWork;
    private readonly UserManager<ApplicationUser> _userManager;

    public SwipeModel(
        ILogger<SwipeModel> logger,
        UserManager<ApplicationUser> userManager,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _userManager =  userManager;
        _unitOfWork = unitOfWork;
    }
    
    public List<Models.Entities.Resume> Resumes { get; set; } = [];
    
    public async Task OnGet()
    {
        var resumes = await _unitOfWork.ResumeRepository
            .GetQuery()
            .Where(v => v.IsVisible)
            .Take(10)
            .ToListAsync();
        
        var currentUser = await _userManager.GetUserAsync(User);

        if (currentUser?.Resume is { IsVisible: true })
        {
            resumes = resumes
                .Where(v => v.Id != currentUser.ResumeId)
                .ToList();
        }

        Resumes = resumes.ToList();
    }
}