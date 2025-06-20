using Aeon_Web.Areas.Project.Pages.Likes.DTOs;
using Aeon_Web.Models.Entities;

namespace Aeon_Web.Services.Abstractions;

public interface IMapper
{
    public LikeDto MapLikeToDto(Like like);
}