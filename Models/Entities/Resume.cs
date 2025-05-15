namespace Aeon_Web.Models.Entities;

public class Resume
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string FullName { get; set; } = string.Empty;
    
    public string Title { get; set; } = string.Empty;
    
    public string Summary { get; set; } = string.Empty;
    
    public List<string> Skills { get; set; } = new();
    
    public ContactInfo Contact { get; set; } = new();
}
