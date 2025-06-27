using System.Linq.Expressions;
using Aeon_Web.Models.Entities;

namespace Aeon_Web.Data.Repository;

public class ResumeSkillRepository(ApplicationDbContext context) : GenericRepository<ResumeSkill>(context)
{
    public bool Any(Expression<Func<ResumeSkill, bool>> predicate)
    {
        return EntityDbSet.Any(predicate);
    }
    
    public IEnumerable<ResumeSkill> FindAll(Expression<Func<ResumeSkill, bool>> predicate)
    {
        return EntityDbSet.Where(predicate).ToArray();
    }

    public void DeleteRange(IEnumerable<ResumeSkill> resumeSkills)
    {
        foreach (var rs in resumeSkills)
        {
            Delete(rs);
        }
    }
}