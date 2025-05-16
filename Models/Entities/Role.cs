using Microsoft.AspNetCore.Identity;

namespace Aeon_Web.Models.Entities;

public class Role : IdentityRole<Guid>
{
    public override Guid Id { get; set; }
}