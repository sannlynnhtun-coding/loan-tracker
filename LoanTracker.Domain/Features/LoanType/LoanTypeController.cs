namespace LoanTracker.Application.Services;

[ApiController]
[Route("api/[controller]")]
public class LoanTypeController : ControllerBase
{
    private readonly LoanTypeService _loanTypeService;

    public LoanTypeController(LoanTypeService loanTypeService)
    {
        _loanTypeService = loanTypeService;
    }

    [HttpPost]
    public async Task<IResult> AddLoanType([FromBody] AddLoanTypeRequest request)
    {
        var result = await _loanTypeService.AddLoanTypeAsync(request.LoanType!, request.BurmeseLoanType!);
        return result.Execute();
    }

    [HttpGet("{id}")]
    public async Task<IResult> GetLoanType(int id)
    {
        var result = await _loanTypeService.GetLoanTypeByIdAsync(id);
        return result.Execute();
    }

    [HttpGet]
    public async Task<IResult> GetAllLoanTypes()
    {
        var result = await _loanTypeService.GetAllLoanTypesAsync();
        return result.Execute();
    }

    [HttpPut("{id}")]
    public async Task<IResult> UpdateLoanType(int id, [FromBody] TblLoanType loanType)
    {
        loanType.LoanTypeId = id;
        var result = await _loanTypeService.UpdateLoanTypeAsync(loanType);
        return result.Execute();
    }

    [HttpDelete("{id}")]
    public async Task<IResult> DeleteLoanType(int id)
    {
        var result = await _loanTypeService.DeleteLoanTypeAsync(id);
        return result.Execute();
    }
}

public class AddLoanTypeRequest
{
    public TblLoanType? LoanType { get; set; }
    public TblLoanTypeBurmese? BurmeseLoanType { get; set; }
}