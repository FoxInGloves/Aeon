using Aeon_Web.Data.Repository.Abstractions;
using Aeon_Web.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Aeon_Web.Areas.Project.Pages.Resume;

public class ListModel : PageModel
{
    private readonly ILogger<ListModel> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<ApplicationUser> _userManager;

    public ListModel(ILogger<ListModel> logger,
        IUnitOfWork unitOfWork,
        UserManager<ApplicationUser> userManager)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _userManager = userManager;
    }
    
    public List<Models.Entities.Resume> Resumes { get; set; } = [];

    [BindProperty(SupportsGet = true)]
    public string? SearchQuery { get; set; }
    
    public async Task OnGetAsync()
    {
        var resumes = await GetFilteredVacanciesAsync();
        var currentUser = await _userManager.GetUserAsync(User);

        if (currentUser?.OwnedVacancy is { IsVisible: true })
        {
            resumes = resumes
                .Where(v => v.Id != currentUser.ResumeId)
                .ToList();
        }

        Resumes = resumes;
    }

    private async Task<List<Models.Entities.Resume>> GetFilteredVacanciesAsync()
    {
        var query = await _unitOfWork.ResumeRepository.GetAsync(v => v.IsVisible);

        if (!string.IsNullOrWhiteSpace(SearchQuery))
        {
            query = query.Where(v =>
                v.Title.Contains(SearchQuery!) ||
                v.Summary.Contains(SearchQuery!) ||
                v.ResumeSkills.Any(vs => vs.Skill.Name.Contains(SearchQuery!)));
        }

        return query.ToList();
    }
}