using Aeon_Web.Data.Repository.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Aeon_Web.Areas.Project.Controllers;

[Area("Project")]
[ApiController]
[Route("api/project")]
public class ResumeController :  Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public ResumeController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet("resume")]
    public async Task<IActionResult> GetResume([FromQuery] int skip = 0, [FromQuery] int take = 10)
    {
        var resumes = await _unitOfWork.ResumeRepository
            .GetQuery()
            .Where(v => v.IsVisible)
            .Skip(skip)
            .Take(take)
            .ToListAsync();

        return PartialView("_ResumeCard", resumes);
    }
}