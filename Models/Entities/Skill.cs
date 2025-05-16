namespace Aeon_Web.Models.Entities;

public class Skill
{
    public required Guid Id { get; set; }
    
    public string Name { get; set; } = string.Empty;

    /*public Guid ResumeId { get; set; }*/
}