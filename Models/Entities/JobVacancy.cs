namespace Aeon_Web.Models.Entities;

public class Vacancy
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    
    public byte DifficultyLevel { get; set; }

    public List<string> SkillsRequired { get; set; } = [];
    
    public DateTime PostedDate { get; set; } = DateTime.UtcNow;
    
    public DateTime? ExpirationDate { get; set; }
    
    public ContactInfo Contact { get; set; } = new();
}
