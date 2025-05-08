using Aeon_Web.Models.Entities;

namespace Aeon_Web.Models.ViewModels;

public class VacancyListViewModel
{
    public required IEnumerable<Vacancy>? Vacancies { get; set; }
    
    public required int CurrentPage { get; set; }
    
    public required int TotalPages { get; set; }
}