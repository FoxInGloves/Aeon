namespace Aeon_Web.Models.Entities;

public class Like
{
    public required Guid Id { get; set; }

    
    public required Guid FromEntityId { get; set; }
    public required LikeEntityType FromEntityType { get; set; }

    public required Guid ToEntityId { get; set; }
    public required LikeEntityType ToEntityType { get; set; }

    public bool IsMatch { get; set; } = false;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public enum LikeEntityType
{
    Vacancy,
    User
}