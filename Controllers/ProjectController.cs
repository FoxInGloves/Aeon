using Aeon_Web.Data.Repository.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Aeon_Web.Controllers;

[ApiController]
/*[Route("api/project")]*/
public class ProjectController : Controller
{
    private readonly IUnitOfWork _unitOfWork;

    public ProjectController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }    
    
    [HttpGet("list")]
    public async Task<IActionResult> GetVacancies([FromQuery] int skip = 0, [FromQuery] int take = 5)
    {
        var vacancies = await _unitOfWork.VacancyRepository
            .GetQuery()
            .Where(v => v.IsVisible)
            .OrderByDescending(v => v.PostedDate)
            .Skip(skip)
            .Take(take)
            .ToListAsync();

        return PartialView("~/", vacancies);
    }
}