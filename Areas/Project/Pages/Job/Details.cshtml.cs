using Aeon_Web.Data.Repository.Abstractions;
using Aeon_Web.Models.Entities;
using Aeon_Web.Services.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Aeon_Web.Areas.Project.Pages.Job;

public class DetailsModel : PageModel
{
    private readonly ILogger<DetailsModel> _logger;
    private readonly ILikeService _likeService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUnitOfWork _unitOfWork;
    
    public DetailsModel(
        ILogger<DetailsModel> logger,
        ILikeService likeService,
        IUnitOfWork unitOfWork,
        UserManager<ApplicationUser> userManager)
    {
        _logger = logger;
        _likeService = likeService;
        _unitOfWork = unitOfWork;
        _userManager = userManager;
    }

    [BindProperty] 
    public Vacancy Vacancy { get; set; }
    
    public bool IsLiked { get; set; }
    
    public bool IsMatch { get; set; }

    public async Task OnGetAsync(Guid vacancyId, bool isMatch = false)
    {
        IsMatch = isMatch;

        var vacancyOwnerEnumerable = await _unitOfWork.UserRepository
            .GetAsync(u => u.OwnedVacancyId == vacancyId);
        var applicationUsers = vacancyOwnerEnumerable as ApplicationUser[] ?? vacancyOwnerEnumerable.ToArray();
        var vacancyOwner = applicationUsers.First();
        
        var user = await _userManager.GetUserAsync(User);
        if (user is null)
        {
            return;
        }
        
        var likesThisVacancyEnumerable = await _unitOfWork.LikeRepository
            .GetAsync(l => l.FromUserId == user.Id &&
                           l.FromEntityType == LikeEntityType.Resume &&
                           l.TargetType == LikeEntityType.Vacancy &&
                           l.ToUserId == vacancyOwner.Id);
        
        var likesThisVacancy = likesThisVacancyEnumerable as Like[] ?? likesThisVacancyEnumerable.ToArray();
        IsLiked = likesThisVacancy.Length > 0;
        
        var vacancy = await _unitOfWork.VacancyRepository.GetByIdAsync(vacancyId);
        if (vacancy is null)
        {
            return;
        }
        
        Vacancy = vacancy;
    }

    public async Task OnPostLikeAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user is null)
        {
            return;
        }

        var vacancyOwnerEnumerable = await _unitOfWork.UserRepository
            .GetAsync(u => u.OwnedVacancyId == Vacancy.Id);
        var applicationUsers = vacancyOwnerEnumerable as ApplicationUser[] ?? vacancyOwnerEnumerable.ToArray();
        var vacancyOwner = applicationUsers.First();
        
        if (user.ResumeId is null)
        {
            
        }
        
        //TODO Проверять на match
        var isMatch = await _likeService.LikeAsync(user.Id, LikeEntityType.Resume, vacancyOwner.Id, LikeEntityType.Vacancy);
    }
}