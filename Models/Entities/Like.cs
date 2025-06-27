namespace Aeon_Web.Models.Entities;

public class Like
{
    public required Guid Id { get; set; }

    
    public required Guid FromUserId { get; set; }
    
    public EntityType FromEntityType { get; set; }

    public required string FromEntityName { get; set; } 
    
    public required Guid ToUserId { get; set; }
    
    public required string ToEntityTitle { get; set; }
    
    public required EntityType TargetType { get; set; }

    public bool IsMatch { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public virtual ApplicationUser FromUser { get; set; }

    public virtual ApplicationUser ToUser { get; set; }
}