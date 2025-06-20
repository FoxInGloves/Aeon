namespace Aeon_Web.Models.Entities;

public class Report
{
    public required Guid Id { get; set; }
    
    public required Guid EntityId { get; set; }
    
    public required EntityType EntityType { get; set; }
}