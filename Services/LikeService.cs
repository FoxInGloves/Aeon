using Aeon_Web.Data.Repository.Abstractions;
using Aeon_Web.Models.Entities;
using Aeon_Web.Services.Abstractions;

namespace Aeon_Web.Services;

public class LikeService : ILikeService
{
    private readonly IUnitOfWork _unitOfWork;

    public LikeService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> LikeAsync(Guid fromId, LikeEntityType fromType, Guid toId, LikeEntityType toType)
    {
        // Проверка, не ставил ли уже лайк
        var existingLike = await _unitOfWork.LikeRepository
            .GetAsync(l => l.FromEntityId == fromId &&
                           l.FromEntityType == fromType &&
                           l.ToEntityId == toId &&
                           l.ToEntityType == toType);

        var enumerable = existingLike as Like[] ?? existingLike.ToArray();

        if (enumerable.Length != 0)
        {
            return enumerable.First().IsMatch;
        }

        // Проверка, есть ли обратный лайк
        var reverseLike = await _unitOfWork.LikeRepository
            .GetAsync(l => l.FromEntityId == toId &&
                           l.FromEntityType == toType &&
                           l.ToEntityId == fromId &&
                           l.ToEntityType == fromType);

        var likes = reverseLike as Like[] ?? reverseLike.ToArray();
        var isMatch = likes.Length != 0;

        // Создание текущего лайка
        var like = new Like
        {
            Id = Guid.NewGuid(),
            FromEntityId = fromId,
            FromEntityType = fromType,
            ToEntityId = toId,
            ToEntityType = toType,
            IsMatch = isMatch
        };

        await _unitOfWork.LikeRepository.CreateAsync(like);

        if (isMatch)
        {
            // Обновление обратного лайка — теперь он тоже match
            var reverse = likes.First();
            reverse.IsMatch = true;
            await _unitOfWork.LikeRepository.UpdateAsync(reverse);
        }

        await _unitOfWork.SaveChangesAsync();

        return isMatch;
    }

    public async Task<IEnumerable<Like>> GetMatchesAsync(Guid entityId, LikeEntityType entityType)
    {
        return await _unitOfWork.LikeRepository
            .GetAsync(l => l.IsMatch &&
                           ((l.FromEntityId == entityId && l.FromEntityType == entityType) ||
                            (l.ToEntityId == entityId && l.ToEntityType == entityType)));
    }
}