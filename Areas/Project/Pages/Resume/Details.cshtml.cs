using Aeon_Web.Data.Repository.Abstractions;
using Aeon_Web.Models.Entities;
using Aeon_Web.Services.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Aeon_Web.Areas.Project.Pages.Resume;

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
    
    public Models.Entities.Resume Resume { get; set; }
    
    public bool IsMatch { get; set; }
    
    public async Task OnGetAsync(Guid resumeId, bool isMatch = false)
    {
        IsMatch = isMatch;
        
        var resume = await _unitOfWork.ResumeRepository.GetByIdAsync(resumeId);
        if (resume == null)
        {
            return;
        }
        
        Resume = resume;
    }
}