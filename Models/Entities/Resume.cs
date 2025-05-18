namespace Aeon_Web.Models.Entities;

public class Resume
{
    public required Guid Id { get; set; }

    public string FullName { get; set; } = string.Empty;
    
    public string Title { get; set; } = string.Empty;
    
    public string Summary { get; set; } = string.Empty;
    
    public virtual ICollection<Skill> Skills { get; set; } = new List<Skill>();
    
    public ContactInfo Contact { get; set; } = new();
}
