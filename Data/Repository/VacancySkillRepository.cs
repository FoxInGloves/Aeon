using System.Linq.Expressions;
using Aeon_Web.Models.Entities;

namespace Aeon_Web.Data.Repository;

public class VacancySkillRepository(ApplicationDbContext context) : GenericRepository<VacancySkill>(context)
{
    public bool Any(Expression<Func<VacancySkill, bool>> predicate)
    {
        return EntityDbSet.Any(predicate);
    }
}