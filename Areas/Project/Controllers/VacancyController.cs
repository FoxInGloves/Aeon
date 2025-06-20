using Aeon_Web.Data.Repository.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Aeon_Web.Areas.Project.Controllers;

[Area("Project")]
[ApiController]
[Route("api/project")]
public class VacancyController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public VacancyController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet("vacancy")]
    public async Task<IActionResult> GetVacancies([FromQuery] int skip = 0, [FromQuery] int take = 10)
    {
        var vacancies = await _unitOfWork.VacancyRepository
            .GetQuery()
            .Where(v => v.IsVisible)
            .OrderByDescending(v => v.PostedDate)
            .Skip(skip)
            .Take(take)
            .ToListAsync();

        return PartialView("_VacancyCard", vacancies);
    }
}