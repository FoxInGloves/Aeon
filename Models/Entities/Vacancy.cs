namespace Aeon_Web.Models.Entities;

public class Vacancy
{
    public required Guid Id { get; set; }

    public required string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    
    public byte DifficultyLevel { get; set; }

    public List<Skill> SkillsRequired { get; set; } = [];
    
    public DateTime PostedDate { get; set; } = DateTime.UtcNow;
    
    public DateTime? ExpirationDate { get; set; }
    
    public ContactInfo Contact { get; set; } = new();
}
