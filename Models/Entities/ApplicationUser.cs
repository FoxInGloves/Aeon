using Microsoft.AspNetCore.Identity;

namespace Aeon_Web.Models.Entities;

public class ApplicationUser : IdentityUser<Guid>
{
    public override Guid Id { get; set; }

    public Resume? Resume { get; set; }
}