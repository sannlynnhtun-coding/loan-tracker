namespace LoanTracker.Domain.Features.CustomerLoan;

[ApiController]
[Route("api/[controller]")]
public class CustomerLoanController : ControllerBase
{
    private readonly CustomerLoanService _customerLoanService;

    public CustomerLoanController(CustomerLoanService customerLoanService)
    {
        _customerLoanService = customerLoanService;
    }

    [HttpPost]
    public async Task<IResult> CreateLoan([FromBody] TblCustomerLoan loan)
    {
        var result = await _customerLoanService.CreateLoanAsync(loan);
        return result.Execute();
    }

    [HttpGet("{id}")]
    public async Task<IResult> GetLoan(int id)
    {
        var result = await _customerLoanService.GetLoanByIdAsync(id);
        return result.Execute();
    }

    [HttpPut("{id}")]
    public async Task<IResult> UpdateLoan(int id, [FromBody] TblCustomerLoan loan)
    {
        loan.LoanId = id;
        var result = await _customerLoanService.UpdateLoanAsync(id, loan);
        return result.Execute();
    }

    [HttpDelete("{id}")]
    public async Task<IResult> DeleteLoan(int id)
    {
        var result = await _customerLoanService.DeleteLoanAsync(id);
        return result.Execute();
    }
}