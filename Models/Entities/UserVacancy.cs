namespace Aeon_Web.Models.Entities;

public class UserVacancy
{
    public required Guid UserId { get; set; }
    public required Guid VacancyId { get; set; }

    public bool LikedByUser { get; set; }
    public bool LikedByVacancyOwner { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual ApplicationUser User { get; set; } = null!;
    public virtual Vacancy Vacancy { get; set; } = null!;
}
