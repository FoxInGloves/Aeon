using Aeon_Web.Data.Repository.Abstractions;
using Aeon_Web.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Aeon_Web.Areas.Identity.Pages.Account.Manage;

public class ViewReportsModel : PageModel
{
    private readonly IUnitOfWork _unitOfWork;

    public ViewReportsModel(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public List<Report> Reports { get; private set; } = new List<Report>();
    
    public async Task OnGetAsync()
    {
        var reports = await _unitOfWork.ReportRepository.GetAsync();
        Reports = reports.ToList();
    }
    
    public string? GetEntityLink(Report report)
    {
        return report.EntityType switch
        {
            EntityType.Vacancy => Url.Page("/Project/Job/Details", "Get", new { area = "Project", vacancyId = report.EntityId }),
            EntityType.Resume => Url.Page("/Resume/Details", "Get", new { area = "User", resumeId = report.EntityId }),
            _ => "#"
        };
    }

}