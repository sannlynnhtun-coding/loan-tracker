namespace LoanTracker.Domain.Features.LateFee;

[ApiController]
[Route("api/[controller]")]
public class LateFeeController : ControllerBase
{
    private readonly LateFeeService _lateFeeService;

    public LateFeeController(LateFeeService lateFeeService)
    {
        _lateFeeService = lateFeeService;
    }

    [HttpPost]
    public async Task<IResult> AddLateFee([FromBody] TblLateFee lateFee)
    {
        var result = await _lateFeeService.AddLateFeeAsync(lateFee);
        return result.Execute();
    }

    [HttpGet("{id}")]
    public async Task<IResult> GetLateFee(int id)
    {
        var result = await _lateFeeService.GetLateFeeByIdAsync(id);
        return result.Execute();
    }
}
