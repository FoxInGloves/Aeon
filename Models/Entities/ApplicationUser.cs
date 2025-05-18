using Microsoft.AspNetCore.Identity;

namespace Aeon_Web.Models.Entities;

public class ApplicationUser : IdentityUser<Guid>
{
    public Guid? ResumeId { get; set; }
    
    public Guid? VacancyId { get; set; }

    public virtual ICollection<Vacancy> FavoriteVacancies { get; set; } = new List<Vacancy>();
    
    public virtual Resume? Resume { get; set; }
    
    public virtual Vacancy? Vacancy { get; set; }
}