using Aeon_Web.Models.Entities;

namespace Aeon_Web.Services.Abstractions;

public interface ILikeService
{
    public Task<bool> LikeAsync(Guid fromId, EntityType fromType, Guid toId, EntityType toType);

    public Task<IEnumerable<Like>> GetMatchesAsync(Guid entityId, EntityType entityType);
}