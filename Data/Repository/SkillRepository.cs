using Aeon_Web.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Aeon_Web.Data.Repository;

public class SkillRepository(ApplicationDbContext context) : GenericRepository<Skill>(context)
{
    public async Task<Skill> GetOrCreateSkillAsync(string name)
    {
        var trimmedName = name.Trim();

        var existingSkill = await EntityDbSet
            .FirstOrDefaultAsync(s => s.Name == trimmedName);

        if (existingSkill != null)
            return existingSkill;

        var newSkill = new Skill
        {
            Id = Guid.NewGuid(),
            Name = trimmedName
        };

        context.Add(newSkill);
        await context.SaveChangesAsync();

        return newSkill;
    }
}