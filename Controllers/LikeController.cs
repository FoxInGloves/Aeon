using Aeon_Web.Data.Repository.Abstractions;
using Aeon_Web.Models.DTOs;
using Aeon_Web.Models.Entities;
using Aeon_Web.Services.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Aeon_Web.Controllers;

[ApiController]
[Route("api/like")]
public class LikeController : Controller
{
    private readonly ILikeService _likeService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<LikeController> _logger;

    public LikeController(
        ILikeService likeService,
        UserManager<ApplicationUser> userManager,
        IUnitOfWork unitOfWork,
        ILogger<LikeController> logger)
    {
        _likeService = likeService;
        _userManager = userManager;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    [HttpPost("vacancy")]
    public async Task<IActionResult> LikeVacancy([FromBody] LikeVacancyRequestDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var currentUser = await _userManager.GetUserAsync(User);
        if (currentUser is null)
            return Unauthorized(new { success = false, error = "Пользователь не найден" });

        var toUser = (await _unitOfWork.UserRepository
                .GetAsync(u => u.OwnedVacancyId == dto.ToVacancyId))
            .FirstOrDefault();

        if (toUser is null)
            return NotFound(new { success = false, error = "Владелец вакансии не найден" });

        var isMatch = await _likeService.LikeAsync(
            fromId: currentUser.Id,
            fromType: EntityType.Resume,
            toId: toUser.Id,
            toType: EntityType.Vacancy);

        _logger.LogInformation("User {FromId} liked Vacancy {ToId}. Match: {IsMatch}", currentUser.Id, dto.ToVacancyId, isMatch);

        return Ok(new { success = true, isMatch });
    }

    [HttpPost("resume")]
    public async Task<IActionResult> LikeResume([FromBody] LikeResumeRequestDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var currentUser = await _userManager.GetUserAsync(User);
        if (currentUser is null)
            return Unauthorized(new { success = false, error = "Пользователь не найден" });

        var toUser = (await _unitOfWork.UserRepository
                .GetAsync(u => u.ResumeId == dto.ToResumeId))
            .FirstOrDefault();

        if (toUser is null)
            return NotFound(new { success = false, error = "Владелец резюме не найден" });

        var isMatch = await _likeService.LikeAsync(
            fromId: currentUser.Id,
            fromType: EntityType.Vacancy,
            toId: toUser.Id,
            toType: EntityType.Resume);

        _logger.LogInformation("User {FromId} liked Resume {ToId}. Match: {IsMatch}", currentUser.Id, dto.ToResumeId, isMatch);

        return Ok(new { success = true, isMatch });
    }
}