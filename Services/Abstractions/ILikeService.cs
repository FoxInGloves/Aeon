using Aeon_Web.Models.Entities;

namespace Aeon_Web.Services.Abstractions;

public interface ILikeService
{
    public Task<bool> LikeAsync(Guid fromId, LikeEntityType fromType, Guid toId, LikeEntityType toType);

    public Task<IEnumerable<Like>> GetMatchesAsync(Guid entityId, LikeEntityType entityType);
}