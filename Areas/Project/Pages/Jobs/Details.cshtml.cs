using Aeon_Web.Models.Entities;
using Aeon_Web.Services.Abstractions;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Aeon_Web.Areas.Project.Pages.Jobs;

public class DetailsModel : PageModel
{
    private readonly ILogger<DetailsModel> _logger;
    private readonly ILikeService _likeService;

    public DetailsModel(ILogger<DetailsModel> logger, ILikeService likeService)
    {
        _logger = logger;
        _likeService = likeService;
    }
    
    public Vacancy Job { get; set; }

    public void OnGet(Guid id)
    {
        // Замените на реальное получение данных из БД
        Job = new Vacancy
        {
            Id = Guid.NewGuid(),
            Title = "Senior .NET Developer",
            Description = "We're looking for an experienced .NET developer to join our team.",
            DifficultyLevel = 1,
            PostedDate = DateTime.UtcNow,
            Contact = new ContactInfo
            {
                Email = "hr@techcorp.com",
                Phone = "+123456789"
            }
        };
    }

    public void OnLikeAsync()
    {
        
    }
}