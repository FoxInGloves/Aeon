using Aeon_Web.Data.Repository.Abstractions;
using Aeon_Web.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Aeon_Web.Areas.Project.Controllers;

[Area("Project")]
[ApiController]
[Route("api/project")]
public class ResumeController :  Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<ApplicationUser> _userManager;

    public ResumeController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
    }

    [HttpGet("resume")]
    public async Task<IActionResult> GetResume([FromQuery] int skip = 0, [FromQuery] int take = 10)
    {
        List<Resume> resumes;
        
        var user = await _userManager.GetUserAsync(User);
        if (user?.ResumeId != null && user.Resume.IsVisible)
        {
            resumes = await _unitOfWork.ResumeRepository
                .GetQuery()
                .Where(r => r.IsVisible && r.Id != user.ResumeId)
                .OrderByDescending(v => v.CreatedAt)
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }
        else
        {
            resumes = await _unitOfWork.ResumeRepository
                .GetQuery()
                .Where(v => v.IsVisible)
                .OrderByDescending(v => v.CreatedAt)
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }

        return PartialView("_ResumeCard", resumes);
    }
}