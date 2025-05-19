using Aeon_Web.Data.Repository.Abstractions;
using Aeon_Web.Models.Entities;

namespace Aeon_Web.Data.Repository;

public class UnitOfWork(ApplicationDbContext context) : IUnitOfWork
{
    private IGenericRepository<ApplicationUser>? _userRepository;
    private IGenericRepository<ApplicationRole>? _roleRepository;
    private IGenericRepository<Resume>? _resumeRepository;
    private IGenericRepository<Vacancy>? _vacancyRepository;
    private SkillRepository? _skillRepository;
    private ResumeSkillRepository? _resumeSkillsRepository;
    private VacancySkillRepository? _vacancySkillRepository;

    public IGenericRepository<ApplicationUser> UserRepository =>
        _userRepository ?? new GenericRepository<ApplicationUser>(context);

    public IGenericRepository<ApplicationRole> RoleRepository =>
        _roleRepository ?? new GenericRepository<ApplicationRole>(context);

    public IGenericRepository<Resume> ResumeRepository =>
        _resumeRepository ?? new GenericRepository<Resume>(context);

    public IGenericRepository<Vacancy> VacancyRepository =>
        _vacancyRepository ?? new GenericRepository<Vacancy>(context);

    public SkillRepository SkillRepository =>
        _skillRepository ?? new SkillRepository(context);

    public ResumeSkillRepository ResumeSkillRepository =>
        _resumeSkillsRepository ?? new ResumeSkillRepository(context);

    public VacancySkillRepository VacancySkillRepository =>
        _vacancySkillRepository ?? new VacancySkillRepository(context);

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }
}