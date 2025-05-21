using System.Diagnostics;
using Aeon_Web.Data.Repository;
using Aeon_Web.Data.Repository.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Aeon_Web.Models;
using Aeon_Web.Models.Entities;
using Aeon_Web.Models.ViewModels;

namespace Aeon_Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    
    private readonly IUnitOfWork _unitOfWork;

    public HomeController(ILogger<HomeController> logger, IUnitOfWork  unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}