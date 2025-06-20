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

    public async Task<bool> LikeAsync(Guid fromUserId, LikeEntityType fromType, Guid toUserId, LikeEntityType toType)
    {
        // Проверка, не ставил ли уже лайк
        var existingLike = await _unitOfWork.LikeRepository
            .GetAsync(l => l.FromUserId == fromUserId &&
                           l.FromEntityType == fromType &&
                           l.ToUserId == toUserId &&
                           l.TargetType == toType);

        var enumerable = existingLike as Like[] ?? existingLike.ToArray();

        if (enumerable.Length != 0)
        {
            return enumerable.First().IsMatch;
        }

        // Проверка, есть ли обратный лайк
        var reverseLike = await _unitOfWork.LikeRepository
            .GetAsync(l => l.FromUserId == toUserId &&
                           l.FromEntityType == toType &&
                           l.ToUserId == fromUserId &&
                           l.TargetType == fromType);

        var likes = reverseLike as Like[] ?? reverseLike.ToArray();
        var isMatch = likes.Length != 0;

        string fromEntityName;
        string toEntityName;
        
        var fromUser = await _unitOfWork.UserRepository.GetByIdAsync(fromUserId);
        var toUser = await _unitOfWork.UserRepository.GetByIdAsync(toUserId);

        if (fromUser is null)
        {
            throw new ArgumentNullException(fromUserId.ToString());
        }

        if (toUser is null)
        {
            throw new ArgumentNullException(toUserId.ToString());
        }
        
        if (fromType == LikeEntityType.Resume)
        {
            if (fromUser.ResumeId is null)
            {
                throw new NullReferenceException(fromUser.ResumeId.ToString());
            }
            
            var fromResume = await _unitOfWork.ResumeRepository.GetByIdAsync(fromUser.ResumeId);
            if (fromResume is null)
            {
                throw new ArgumentNullException(fromUser.ResumeId.ToString());
            }
            
            fromEntityName = fromResume.FullName;

            if (toUser.OwnedVacancyId is null)
            {
                throw new NullReferenceException(toUser.OwnedVacancyId.ToString());
            }
            
            var toVacancy = await _unitOfWork.VacancyRepository.GetByIdAsync(toUser.OwnedVacancyId);
            if (toVacancy is null)
            {
                throw new NullReferenceException(toUser.OwnedVacancyId.ToString());
            }

            toEntityName = toVacancy.Title;
        }
        else
        {
            if (fromUser.OwnedVacancyId is null)
            {
                throw new NullReferenceException(fromUser.OwnedVacancyId.ToString());
            }
            
            var vacancy = await _unitOfWork.VacancyRepository.GetByIdAsync(fromUser.OwnedVacancyId);
            if (vacancy is null)
            {
                throw new ArgumentNullException(fromUser.OwnedVacancyId.ToString());
            }

            fromEntityName = vacancy.Title;

            if (toUser.ResumeId is null)
            {
                throw new NullReferenceException(toUser.ResumeId.ToString());
            }
            
            var toResume = await _unitOfWork.ResumeRepository.GetByIdAsync(toUser.ResumeId);
            if (toResume is null)
            {
                throw new ArgumentNullException(toUser.ResumeId.ToString());
            }
            
            toEntityName = toResume.FullName;
        }
        
        // Создание текущего лайка
        var like = new Like
        {
            Id = Guid.NewGuid(),
            FromUserId = fromUserId,
            FromEntityType = fromType,
            ToUserId = toUserId,
            TargetType = toType,
            IsMatch = isMatch,
            FromEntityName = fromEntityName,
            ToEntityTitle = toEntityName
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
                           ((l.FromUserId == entityId && l.FromEntityType == entityType) ||
                            (l.ToUserId == entityId && l.TargetType == entityType)));
    }
}