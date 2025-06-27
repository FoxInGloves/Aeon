using Aeon_Web.Models.Entities;

namespace Aeon_Web.Data.Repository.Abstractions;

public interface IUnitOfWork
{
    public IGenericRepository<ApplicationUser> UserRepository { get; }
    
    public IGenericRepository<ApplicationRole> RoleRepository { get; }
    
    public IGenericRepository<Resume> ResumeRepository { get; }
    
    public IGenericRepository<Vacancy> VacancyRepository { get; }
    
    public SkillRepository SkillRepository { get; }
    
    public ResumeSkillRepository ResumeSkillRepository { get; }
    
    public VacancySkillRepository VacancySkillRepository { get; }
    
    public IGenericRepository<Like> LikeRepository { get; }
    
    public IGenericRepository<Report> ReportRepository { get; }
    
    public Task SaveChangesAsync();
}