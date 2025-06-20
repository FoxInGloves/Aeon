using Aeon_Web.Areas.Project.Pages.Likes.DTOs;
using Aeon_Web.Models.Entities;
using Aeon_Web.Services.Abstractions;

namespace Aeon_Web.Services;

public class Mapper : IMapper
{
    public LikeDto MapLikeToDto(Like like)
    {
        var likeDto = new LikeDto
        {
            LikeId = like.Id,
            FromEntityId = like.FromEntityType == LikeEntityType.Resume ? 
                like.FromUser.ResumeId.Value : like.FromUser.OwnedVacancyId.Value,
            FromEntityName = like.FromEntityName,
            TargetType = like.TargetType,
            TargetTitle = like.ToEntityTitle,
            LikedAt = like.CreatedAt,
            IsMatch = like.IsMatch
        };

        return likeDto;
    }
}