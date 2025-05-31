using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Aeon_Web.Models.Entities;

[Owned]
public class ContactInfo
{
    public string Email { get; set; }
    
    public string? Phone { get; set; }
    
    public string? Website { get; set; }
}