namespace Aeon_Web.Services.Abstractions;

public interface IVacancyService
{
    public Task DeleteVacancyAsync(Guid? vacancyId, Guid userId);
}