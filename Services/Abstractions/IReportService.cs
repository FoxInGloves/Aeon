using Aeon_Web.Models.Entities;

namespace Aeon_Web.Services.Abstractions;

public interface IReportService
{
    public Task<List<Report>> GetReports();

    public Task ReportVacancyAsync(Guid vacancyId);

    public Task ReportResumeAsync(Guid resumeId);
}