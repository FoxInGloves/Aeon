using Microsoft.EntityFrameworkCore;

namespace Aeon_Web.Models.Entities;

[Owned]
public class ContactInfo
{
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Website { get; set; } = string.Empty;
}