using Aeon_Web.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Aeon_Web.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
{
    private ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<Resume> Resumes { get; set; }

    public DbSet<Vacancy> Vacancies { get; set; }

    public DbSet<Skill> Skills { get; set; }
    
    public DbSet<ResumeSkill> ResumeSkills { get; set; }

    public DbSet<VacancySkill> VacancySkills { get; set; }
    
    public DbSet<UserVacancy>  UserVacancies { get; set; }
    
    public DbSet<Match> Matches { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ApplicationUser>(b =>
        {
            b.ToTable("Users");

            b.HasOne(u => u.Resume)
                .WithOne()
                .HasForeignKey<ApplicationUser>(u => u.ResumeId)
                .OnDelete(DeleteBehavior.Cascade);

            b.HasOne(u => u.OwnedVacancy)
                .WithOne()
                .HasForeignKey<ApplicationUser>(u => u.OwnedVacancyId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        builder.Entity<Match>(b =>
        {
            b.ToTable("Matches");

            b.HasOne(m => m.User)
                .WithOne()
                .HasForeignKey<Match>(m => m.UserId);

            b.HasOne(m => m.Vacancy)
                .WithOne()
                .HasForeignKey<Match>(m => m.VacancyId);
        });

        builder.Entity<ApplicationRole>(b => { b.ToTable("Roles"); });

        builder.Entity<IdentityUserRole<Guid>>(b => { b.ToTable("UserRoles"); });

        builder.Entity<IdentityUserClaim<Guid>>(b => { b.ToTable("UserClaims"); });

        builder.Entity<IdentityUserLogin<Guid>>(b => { b.ToTable("UserLogins"); });

        builder.Entity<IdentityRoleClaim<Guid>>(b => { b.ToTable("RoleClaims"); });

        builder.Entity<IdentityUserToken<Guid>>(b => { b.ToTable("UserTokens"); });

        builder.Entity<Skill>(b => { b.HasKey(s => s.Id); });
        
        builder.Entity<ResumeSkill>().HasKey(rs => new { rs.ResumeId, rs.SkillId });

        builder.Entity<ResumeSkill>()
            .HasOne(rs => rs.Resume)
            .WithMany(r => r.ResumeSkills)
            .HasForeignKey(rs => rs.ResumeId);

        builder.Entity<ResumeSkill>()
            .HasOne(rs => rs.Skill)
            .WithMany(s => s.ResumeSkills)
            .HasForeignKey(rs => rs.SkillId);
        
        builder.Entity<VacancySkill>().HasKey(vs => new { vs.VacancyId, vs.SkillId });

        builder.Entity<VacancySkill>()
            .HasOne(vs => vs.Vacancy)
            .WithMany(v => v.VacancySkills)
            .HasForeignKey(vs => vs.VacancyId);

        builder.Entity<VacancySkill>()
            .HasOne(vs => vs.Skill)
            .WithMany(s => s.VacancySkills)
            .HasForeignKey(vs => vs.SkillId);
        
        builder.Entity<UserVacancy>().HasKey(vs => new { vs.UserId, vs.VacancyId });

        builder.Entity<UserVacancy>()
            .HasOne(uv => uv.User)
            .WithMany(u => u.UserVacancies)
            .HasForeignKey(uv => uv.UserId);

        builder.Entity<UserVacancy>()
            .HasOne(uv => uv.Vacancy)
            .WithMany(v => v.UserVacancies)
            .HasForeignKey(uv => uv.VacancyId);
        
        builder.Entity<Like>(entity =>
        {
            entity.HasKey(l => l.Id);

            entity.Property(l => l.Id)
                .ValueGeneratedNever(); // если ты сам генерируешь Guid

            entity.Property(l => l.FromEntityId)
                .IsRequired();

            entity.Property(l => l.FromEntityType)
                .IsRequired()
                .HasConversion<string>(); // enum -> string

            entity.Property(l => l.ToEntityId)
                .IsRequired();

            entity.Property(l => l.ToEntityType)
                .IsRequired()
                .HasConversion<string>(); // enum -> string

            entity.Property(l => l.IsMatch)
                .IsRequired();

            /*entity.Property(l => l.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP"); // или DateTime.UtcNow в C#*/

            // Один лайк от сущности к сущности — уникальный
            entity.HasIndex(l => new { l.FromEntityId, l.FromEntityType, l.ToEntityId, l.ToEntityType })
                .IsUnique();
        });
    }
}