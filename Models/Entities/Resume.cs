namespace Aeon_Web.Models.Entities;

public class Resume
{
    public required Guid Id { get; set; }
    
    public bool IsVisible { get; set; }
    
    public string FullName { get; set; }
    
    public string Title { get; set; } = string.Empty;
    
    public string Summary { get; set; } = string.Empty;
    
    public DateTime CreatedAt { get; set; }
    
    public virtual ICollection<ResumeSkill> ResumeSkills { get; set; } = [];
    
    public ContactInfo Contact { get; set; } = new();
}
