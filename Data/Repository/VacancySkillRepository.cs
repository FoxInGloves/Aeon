using System.Linq.Expressions;
using Aeon_Web.Models.Entities;

namespace Aeon_Web.Data.Repository;

public class VacancySkillRepository(ApplicationDbContext context) : GenericRepository<VacancySkill>(context)
{
    public bool Any(Expression<Func<VacancySkill, bool>> predicate)
    {
        return EntityDbSet.Any(predicate);
    }
    
    public IEnumerable<VacancySkill> FindAll(Expression<Func<VacancySkill, bool>> predicate)
    {
        return EntityDbSet.Where(predicate).ToArray();
    }

    public void DeleteRange(IEnumerable<VacancySkill> vacancySkills)
    {
        foreach (var vs in vacancySkills)
        {
            Delete(vs);
        }
    }
}