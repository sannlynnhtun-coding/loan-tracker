using LoanTracker.Domain.Features.LoanType;

[ApiController]
[Route("api/[controller]")]
public class LoanTypeController : ControllerBase
{
    private readonly CreateLoanTypeService _createLoanTypeService;
    private readonly GetLoanTypeService _getLoanTypeService;
    private readonly UpdateLoanTypeService _updateLoanTypeService;
    private readonly DeleteLoanTypeService _deleteLoanTypeService;

    public LoanTypeController(
        CreateLoanTypeService createLoanTypeService,
        GetLoanTypeService getLoanTypeService,
        UpdateLoanTypeService updateLoanTypeService,
        DeleteLoanTypeService deleteLoanTypeService)
    {
        _createLoanTypeService = createLoanTypeService;
        _getLoanTypeService = getLoanTypeService;
        _updateLoanTypeService = updateLoanTypeService;
        _deleteLoanTypeService = deleteLoanTypeService;
    }

    [HttpPost]
    public async Task<IResult> CreateLoanType(CreateLoanTypeRequest request)
    {
        var result = await _createLoanTypeService.HandleAsync(request);
        return result.Execute();
    }

    [HttpGet("{id}")]
    public async Task<IResult> GetLoanType(int id)
    {
        var request = new GetLoanTypeRequest { LoanTypeId = id };
        var result = await _getLoanTypeService.HandleAsync(request);
        return result.Execute();
    }

    [HttpPut("{id}")]
    public async Task<IResult> UpdateLoanType(int id, UpdateLoanTypeRequest request)
    {
        var result = await _updateLoanTypeService.HandleAsync(id, request);
        return result.Execute();
    }

    [HttpDelete("{id}")]
    public async Task<IResult> DeleteLoanType(int id)
    {
        var request = new DeleteLoanTypeRequest { LoanTypeId = id };
        var result = await _deleteLoanTypeService.HandleAsync(request);
        return result.Execute();
    }
}