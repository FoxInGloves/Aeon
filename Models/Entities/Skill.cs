namespace Aeon_Web.Models.Entities;

public class Skill
{
    public required Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;
    
    public virtual ICollection<ResumeSkill> ResumeSkills { get; set; } = new List<ResumeSkill>();
    public virtual ICollection<VacancySkill> VacancySkills { get; set; } = new List<VacancySkill>();
}