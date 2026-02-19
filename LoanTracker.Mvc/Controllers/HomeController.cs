using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using LoanTracker.Mvc.Models;
using LoanTracker.Mvc.Services;
using LoanTracker.Shared.Models;

namespace LoanTracker.Mvc.Controllers;

public class HomeController : Controller
{
    private readonly HttpClientService _httpClientService;
    private readonly ILogger<HomeController> _logger;

    public HomeController(HttpClientService httpClientService, ILogger<HomeController> logger)
    {
        _httpClientService = httpClientService;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var result = await _httpClientService.GetAsync<DashboardResponse>("api/Dashboard");
        return View(result?.Data ?? new DashboardResponse());
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
