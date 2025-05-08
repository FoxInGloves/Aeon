using Aeon_Web.Models.Entities.Resume;

namespace Aeon_Web.Models;

public class JobVacancy
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Title { get; set; } = string.Empty; // Название вакансии
    public string Description { get; set; } = string.Empty; // Подробности, обязанности

    public string CompanyName { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    
    public ExperienceLevel ExperienceLevel { get; set; } // Junior/Mid/Senior

    public List<string> SkillsRequired { get; set; } = new();
    
    public DateTime PostedDate { get; set; } = DateTime.UtcNow;
    public DateTime? ExpirationDate { get; set; }
    
    public ContactInfo Contact { get; set; } = new();
}

public enum ExperienceLevel
{
    Junior,
    Mid,
    Senior,
    Lead
}
