using Aeon_Web.Models.Entities;

namespace Aeon_Web.Data.Repository;

public class ResumeRepository(ApplicationDbContext context) : GenericRepository<Resume>(context)
{
    
}