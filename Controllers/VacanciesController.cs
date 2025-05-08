using Aeon_Web.Models.Entities;
using Aeon_Web.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Aeon_Web.Controllers;

public class VacanciesController : Controller
{
    private readonly ILogger<VacanciesController> _logger;

    public VacanciesController(ILogger<VacanciesController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index(int pageNumber, int pageSize)
    {
        var viewModel = GetJobsViewModel(pageNumber, pageSize);
        
        return View(viewModel);
    }

    public IActionResult Catalog(int pageNumber, int pageSize)
    {
        var viewModel = GetJobsViewModel(pageNumber, pageSize);
        
        return View(viewModel);
    }

    public IActionResult Details(Guid vacancyId)
    {
        //TODO дописать открытие детальной иформации о вакансии
        
        return View(GetFakeJobs(1)[0]);
    }

    private VacancyListViewModel GetJobsViewModel(int pageNumber, int pageSize)
    {
        var vacancies = GetFakeJobs(10);
        var totalJobs = GetFakeJobs(25).Count;

        var viewModel = new VacancyListViewModel
        {
            CurrentPage = pageNumber,
            Vacancies = vacancies,
            TotalPages = (int)Math.Ceiling(totalJobs / (double)pageSize)
        };
        
        return viewModel;
    }

    private List<Vacancy> GetFakeJobs(int count)
    {
        var jobs = new List<Vacancy>();
        for (var i = 0; i < count; i++)
        {
            jobs.Add(Vacancy.CreateVacancy(
                "Some job",
                "non",
                "Нууууу... Тут мы описываем наш проект, что хотим делать и тд",
                "А ищем мы именно тебя!"));
        }

        return jobs;
    }
}