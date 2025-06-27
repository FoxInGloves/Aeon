using Aeon_Web.Data.Repository.Abstractions;
using Aeon_Web.Models.Entities;
using Aeon_Web.Services.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Aeon_Web.Areas.Project.Pages.Resume;

public class DetailsModel : PageModel
{
    private readonly ILogger<DetailsModel> _logger;
    private readonly ILikeService _likeService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IReportService _reportService;
    
    public DetailsModel(
        ILogger<DetailsModel> logger,
        ILikeService likeService,
        IUnitOfWork unitOfWork,
        UserManager<ApplicationUser> userManager,
        IReportService reportService)
    {
        _logger = logger;
        _likeService = likeService;
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _reportService = reportService;
    }
    
    [BindProperty]
    public Models.Entities.Resume Resume { get; set; }
    
    public bool IsMatch { get; set; }
    
    public bool IsLiked { get; set; }
    
    public async Task<IActionResult> OnGetAsync(Guid resumeId, bool isMatch = false)
    {
        IsMatch = isMatch;

        var resume = await _unitOfWork.ResumeRepository.GetByIdAsync(resumeId);
        if (resume is null)
            return RedirectToPage("./List");

        Resume = resume;

        var resumeOwner = (await _unitOfWork.UserRepository
                .GetAsync(u => u.ResumeId == resumeId))
            .FirstOrDefault();

        if (resumeOwner is null)
            return RedirectToPage("./List");

        var user = await _userManager.GetUserAsync(User);
        if (user is null)
            return Challenge();

        var likes = await _unitOfWork.LikeRepository.GetAsync(l =>
            l.FromUserId == user.Id &&
            l.FromEntityType == EntityType.Vacancy &&
            l.TargetType == EntityType.Resume &&
            l.ToUserId == resumeOwner.Id);

        IsLiked = likes.Any();
        
        return Page();
    }
    
    public async Task<IActionResult> OnPostLikeAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user is null)
        {
            return Challenge();
        }

        var resumeOwnerEnumerable = await _unitOfWork.UserRepository
            .GetAsync(u => u.ResumeId == Resume.Id);
        var applicationUsers = resumeOwnerEnumerable as ApplicationUser[] ?? resumeOwnerEnumerable.ToArray();

        if (applicationUsers.Length == 0)
        {
            return RedirectToPage("/Resume/Details", new { area = "Project", vacancyId = Resume.Id });
        }
        var vacancyOwner = applicationUsers.First();

        if (user.ResumeId is null)
        {
            
        }

        bool isMatch;
        //TODO Проверять на match
        try
        {
            isMatch = await _likeService.LikeAsync(user.Id, EntityType.Vacancy, vacancyOwner.Id, EntityType.Resume);
        }
        catch (Exception e)
        {
            _logger.LogError("Like vacancy error {Error}", e);
            TempData["Message"] = "Произошла ошибка";
            throw;
        }
        
        return RedirectToPage("/Resume/Details", new { area = "Project", resumeId = Resume.Id,  isMatch });
    }
    
    public async Task<IActionResult> OnPostReportAsync()
    {
        /*if (!ModelState.IsValid)
            return RedirectToPage(new { Vacancy.Id });*/

        var resumeId = Resume.Id;

        await _reportService.ReportResumeAsync(resumeId);

        TempData["Message"] = "Жалоба успешно отправлена.";
        return RedirectToPage(new { resumeId });
    }
}