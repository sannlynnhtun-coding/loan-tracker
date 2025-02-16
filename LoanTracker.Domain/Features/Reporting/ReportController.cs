using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanTracker.Domain.Features.Reporting;

[ApiController]
[Route("api/[controller]")]
public class ReportController : ControllerBase
{
    private readonly ReportingService _reportingService;

    public ReportController(ReportingService reportingService)
    {
        _reportingService = reportingService;
    }

    [HttpGet("loan-report")]
    public async Task<IResult> GenerateLoanReport()
    {
        var result = await _reportingService.GenerateLoanReportAsync();
        return result.Execute();
    }
}