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

    [BindProperty] public Vacancy Vacancy { get; set; }

    public bool IsLiked { get; set; }

    public bool IsMatch { get; set; }

    public async Task<IActionResult> OnGetAsync(Guid vacancyId, bool isMatch = false)
    {
        IsMatch = isMatch;
        
        var vacancy = await _unitOfWork.VacancyRepository.GetByIdAsync(vacancyId);
        if (vacancy is null)
            return RedirectToPage("./List");

        Vacancy = vacancy;

        var vacancyOwners = await _unitOfWork.UserRepository
            .GetAsync(u => u.OwnedVacancyId == vacancyId);
        var vacancyOwner = vacancyOwners.FirstOrDefault();

        if (vacancyOwner is null)
            return Page();

        var user = await _userManager.GetUserAsync(User);
        if (user is null)
            return Page();

        var likes = await _unitOfWork.LikeRepository.GetAsync(l =>
            l.FromUserId == user.Id &&
            l.FromEntityType == EntityType.Resume &&
            l.TargetType == EntityType.Vacancy &&
            l.ToUserId == vacancyOwner.Id);

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

        var vacancyOwnerEnumerable = await _unitOfWork.UserRepository
            .GetAsync(u => u.OwnedVacancyId == Vacancy.Id);
        var applicationUsers = vacancyOwnerEnumerable as ApplicationUser[] ?? vacancyOwnerEnumerable.ToArray();
        var vacancyOwner = applicationUsers.First();

        if (user.ResumeId is null)
        {
            TempData["Message"] = "Необходимо создать резюме";
            return RedirectToPage("/Account/Profile/Edit", new { area = "Identity" });
        }

        bool isMatch;
        //TODO Проверять на match
        try
        {
            isMatch = await _likeService.LikeAsync(user.Id, EntityType.Resume, vacancyOwner.Id, EntityType.Vacancy);
        }
        catch (Exception e)
        {
            _logger.LogError("Like resume error {Error}", e);
            TempData["Message"] = "Произошла ошибка";
            throw;
        }
        
        return RedirectToPage("/Job/Details", new { area = "Project", vacancyId = Vacancy.Id, isMatch });
    }

    public async Task<IActionResult> OnPostReportAsync()
    {
        /*if (!ModelState.IsValid)
            return RedirectToPage(new { Vacancy.Id });*/

        var vacancyId = Vacancy.Id;

        await _reportService.ReportVacancyAsync(vacancyId);

        TempData["Message"] = "Жалоба успешно отправлена.";
        return RedirectToPage(new { vacancyId });
    }
}