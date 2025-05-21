using Microsoft.AspNetCore.Identity;

namespace Aeon_Web.Models.Entities;

public class ApplicationUser : IdentityUser<Guid>
{
    public Guid? ResumeId { get; set; }
    
    public Guid? OwnedVacancyId { get; set; }

    public virtual ICollection<UserVacancy> UserVacancies { get; set; } = new List<UserVacancy>();
    
    public virtual Resume? Resume { get; set; }
    
    public virtual Vacancy? OwnedVacancy { get; set; }
}