namespace LoanTracker.Domain.Features.CustomerPayment;

[ApiController]
[Route("api/[controller]")]
public class CustomerPaymentController : ControllerBase
{
    private readonly CustomerPaymentService _customerPaymentService;

    public CustomerPaymentController(CustomerPaymentService customerPaymentService)
    {
        _customerPaymentService = customerPaymentService;
    }

    [HttpPost]
    public async Task<IResult> AddPayment([FromBody] TblCustomerPayment payment)
    {
        var result = await _customerPaymentService.AddPaymentAsync(payment);
        return result.Execute();
    }

    [HttpGet("{id}")]
    public async Task<IResult> GetPayment(int id)
    {
        var result = await _customerPaymentService.GetPaymentByIdAsync(id);
        return result.Execute();
    }
}
