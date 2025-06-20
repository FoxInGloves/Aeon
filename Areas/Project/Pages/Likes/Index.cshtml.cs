using Aeon_Web.Areas.Project.Pages.Likes.DTOs;
using Aeon_Web.Data.Repository.Abstractions;
using Aeon_Web.Models.Entities;
using Aeon_Web.Services.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Aeon_Web.Areas.Project.Pages.Likes;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;

    public IndexModel(
        ILogger<IndexModel> logger,
        IUnitOfWork unitOfWork,
        UserManager<ApplicationUser> userManager,
        IMapper mapper)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _mapper = mapper;
    }
    
    public List<LikeDto> Likes { get; set; } = [];

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is null)
            {
                return Challenge();
            }
            var likes = await _unitOfWork.LikeRepository.GetAsync(l => l.ToUserId == user.Id);

            foreach (var like in likes)
            {
                Likes.Add(_mapper.MapLikeToDto(like));
            }
        }
        catch (Exception e)
        {
            _logger.LogError("Unexpected error occured while getting likes: {Error}",  e.Message);
            /*throw;*/
        }

        return Page();
    }
}