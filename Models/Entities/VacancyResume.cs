namespace Aeon_Web.Models.Entities;

public class VacancyResume
{
    public required Guid VacancyId { get; set; }
    
    public required Guid  ResumeId { get; set; }
    
    public DateTime RespondedDate { get; set; }
    
    public virtual Vacancy Vacancy { get; set; }
    
    public virtual Resume Resume { get; set; }
}