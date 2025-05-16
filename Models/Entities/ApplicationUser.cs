using Microsoft.AspNetCore.Identity;

namespace Aeon_Web.Models.Entities;

public class ApplicationUser : IdentityUser<Guid>
{
    public override required Guid Id { get; set; }
    
    public required Guid RoleId { get; set; }

    public Guid ResumeId { get; set; }
    
    public virtual IdentityRole<Guid> Role { get; set; }
    
    public virtual Resume? Resume { get; set; }
}