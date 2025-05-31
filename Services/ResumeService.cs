using Aeon_Web.Data.Repository.Abstractions;
using Aeon_Web.Models.Entities;

namespace Aeon_Web.Services;

public class ResumeService
{
    private readonly ILogger<ResumeService> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public ResumeService(ILogger<ResumeService> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Deletes a resume and its associated data, including related skills, from the database.
    /// </summary>
    /// <param name="resumeId">The unique identifier of the resume to be deleted. If null, the method will perform no action.</param>
    /// <returns>A task that represents the asynchronous delete operation.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the resume with the provided <paramref name="resumeId"/> is not found.</exception>
    public async Task DeleteResumeAsync(Guid? resumeId)
    {
        if (resumeId is null)
            return;

        var resume = await _unitOfWork.ResumeRepository.GetByIdAsync(resumeId);
        if (resume is null)
            throw new InvalidOperationException("Resume not found");
        
        var resumeSkills = _unitOfWork.ResumeSkillRepository
            .FindAll(rs => rs.ResumeId == resumeId).ToList();

        if (resumeSkills.Count != 0)
        {
            _unitOfWork.ResumeSkillRepository.DeleteRange(resumeSkills);
        }
        
        _unitOfWork.ResumeRepository.Delete(resume);
        
        var usedSkillIds = resumeSkills.Select(rs => rs.SkillId).Distinct();

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
        
        await _unitOfWork.SaveChangesAsync();
    }

}