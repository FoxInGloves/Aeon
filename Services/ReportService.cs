using Aeon_Web.Data.Repository.Abstractions;
using Aeon_Web.Models.Entities;
using Aeon_Web.Services.Abstractions;

namespace Aeon_Web.Services;

public class ReportService : IReportService
{
    private readonly IUnitOfWork _unitOfWork;

    public ReportService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<Report>> GetReports()
    {
        var reportsEnumerable = await _unitOfWork.ReportRepository.GetAsync();

        return reportsEnumerable.ToList();
    }
    
    public async Task ReportVacancyAsync(Guid vacancyId)
    {
        await SendReportAsync(vacancyId, EntityType.Vacancy);
    }

    public async Task ReportResumeAsync(Guid resumeId)
    {
        await SendReportAsync(resumeId, EntityType.Resume);
    }

    private async Task SendReportAsync(Guid entityId, EntityType entityType)
    {
        var report = new Report
        {
            Id = Guid.NewGuid(),
            EntityId = entityId,
            EntityType = entityType
        };

        await _unitOfWork.ReportRepository.CreateAsync(report);

        await _unitOfWork.SaveChangesAsync();
    }
}