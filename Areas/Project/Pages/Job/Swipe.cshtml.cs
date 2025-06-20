using Aeon_Web.Data.Repository.Abstractions;
using Aeon_Web.Models.Entities;
using Aeon_Web.Services.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Aeon_Web.Areas.Project.Pages.Job;

public class SwipeModel : PageModel
{
    private readonly ILogger<SwipeModel> _logger;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILikeService _likeService;
    private readonly IUnitOfWork _unitOfWork;

    public SwipeModel(
        ILogger<SwipeModel> logger,
        UserManager<ApplicationUser> userManager,
        ILikeService likeService,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _userManager = userManager;
        _likeService = likeService;
        _unitOfWork = unitOfWork;
    }
    
    public List<Vacancy> Vacancies { get; set; } = [];

    public async Task OnGetAsync()
    {
        var vacancies = await _unitOfWork.VacancyRepository
            .GetQuery()
            .Where(v => v.IsVisible)
            .OrderByDescending(v => v.PostedDate)
            .Take(10)
            .ToListAsync();
        
        var currentUser = await _userManager.GetUserAsync(User);

        if (currentUser?.OwnedVacancy is { IsVisible: true })
        {
            vacancies = vacancies
                .Where(v => v.Id != currentUser.OwnedVacancyId)
                .ToList();
        }

        Vacancies = vacancies.ToList();
    }

    public class LikeRequestDto
    {
        public Guid ToVacancyId { get; set; }
    }
}