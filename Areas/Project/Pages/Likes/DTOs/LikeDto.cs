using Aeon_Web.Models.Entities;

namespace Aeon_Web.Areas.Project.Pages.Likes.DTOs;

public class LikeDto
{
    public required Guid LikeId { get; set; }
    
    public required Guid FromEntityId { get; set; }
    
    public string FromEntityName { get; set; } = string.Empty;
    
    public EntityType TargetType { get; set; }
    
    public string TargetTitle { get; set; } = string.Empty;
    
    public required bool IsMatch { get; set; }
    
    public DateTime LikedAt { get; set; }
}
