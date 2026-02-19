using LoanTracker.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace LoanTracker.Application.Services;

[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly DashboardService _dashboardService;

    public DashboardController(DashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    [HttpGet]
    public async Task<IResult> GetDashboardData()
    {
        var result = await _dashboardService.GetDashboardDataAsync();
        return result.Execute();
    }
}
