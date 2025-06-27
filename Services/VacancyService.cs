using Aeon_Web.Data.Repository.Abstractions;
using Aeon_Web.Models.Entities;
using Aeon_Web.Services.Abstractions;
using Microsoft.AspNetCore.Identity;

namespace Aeon_Web.Services;

public class VacancyService : IVacancyService
{
    private readonly ILogger<ResumeService> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<ApplicationUser> _userManager;

    public VacancyService(
        ILogger<ResumeService> logger,
        IUnitOfWork unitOfWork,
        UserManager<ApplicationUser> userManager)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _userManager = userManager;
    }
    
    public async Task DeleteVacancyAsync(Guid? vacancyId, Guid userId)
    {
        if (vacancyId is null)
            return;

        var vacancy = await _unitOfWork.VacancyRepository.GetByIdAsync(vacancyId);
        if (vacancy is null)
            throw new InvalidOperationException("Vacancy not found");
        
        var vacancySkills = _unitOfWork.VacancySkillRepository
            .FindAll(rs => rs.VacancyId == vacancyId).ToList();

        if (vacancySkills.Count != 0)
        {
            _unitOfWork.VacancySkillRepository.DeleteRange(vacancySkills);
        }
        
        _unitOfWork.VacancyRepository.Delete(vacancy);
        
        var usedSkillIds = vacancySkills.Select(rs => rs.SkillId).Distinct();

        foreach (var skillId in usedSkillIds)
        {
            var skill = await _unitOfWork.SkillRepository.GetByIdAsync(skillId);
            if (skill is null)
                continue;

            var isUsedElsewhere =
                _unitOfWork.ResumeSkillRepository.Any(rs => rs.SkillId == skillId) ||
                _unitOfWork.VacancySkillRepository.Any(vs => vs.SkillId == skillId);

            if (!isUsedElsewhere)
            {
                _unitOfWork.SkillRepository.Delete(skill);
            }
        }
        
        var likes = await _unitOfWork.LikeRepository.GetAsync(l => l.ToUserId == userId ||
                                                             l.FromUserId == userId);
        foreach (var like in likes)
        {
            _unitOfWork.LikeRepository.Delete(like);
        }
        
        await _unitOfWork.SaveChangesAsync();
    }
}