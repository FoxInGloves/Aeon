namespace Aeon_Web.Models.Entities;

public class Vacancy
{
    public required Guid Id { get; set; }

    public bool IsVisible { get; set; }
    
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public byte DifficultyLevel { get; set; }

    public virtual List<VacancySkill> VacancySkills { get; set; } = [];

    public DateTime PostedDate { get; set; } = DateTime.UtcNow;

    public DateTime? ExpirationDate { get; set; }

    public ContactInfo Contact { get; set; } = new();

    public virtual ICollection<UserVacancy> UserVacancies { get; set; } = new List<UserVacancy>();

    //public virtual ICollection<Resume> RespondedResumes { get; set; } = new List<Resume>();
}