namespace Aeon_Web.Models.Entities;

public class ResumeSkill
{
    public required Guid ResumeId { get; set; }

    public required Guid SkillId { get; set; }
    
    
    public virtual Skill? Skill { get; set; }
    
    public virtual Resume? Resume { get; set; }
}