namespace Aeon_Web.Models.Entities;

public class Match
{
    public required Guid Id { get; set; }

    public required Guid UserId { get; set; }
    public required Guid VacancyId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual ApplicationUser User { get; set; } = null!;
    public virtual Vacancy Vacancy { get; set; } = null!;
}