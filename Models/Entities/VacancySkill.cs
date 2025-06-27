namespace Aeon_Web.Models.Entities;

public class VacancySkill
{
    public required Guid VacancyId { get; set; }

    public required Guid SkillId { get; set; }
    
    public virtual Skill? Skill { get; set; }
    
    public virtual Vacancy? Vacancy { get; set; }
}