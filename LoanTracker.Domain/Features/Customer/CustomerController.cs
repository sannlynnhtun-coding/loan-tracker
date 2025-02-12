namespace LoanTracker.Domain.Features.Customer;

[ApiController]
[Route("api/[controller]")]
public class CustomerController : ControllerBase
{
    private readonly CreateCustomerService _createCustomerService;
    private readonly GetCustomerService _getCustomerService;
    private readonly UpdateCustomerService _updateCustomerService;
    private readonly DeleteCustomerService _deleteCustomerService;

    public CustomerController(
        CreateCustomerService createCustomerService,
        GetCustomerService getCustomerService,
        UpdateCustomerService updateCustomerService,
        DeleteCustomerService deleteCustomerService)
    {
        _createCustomerService = createCustomerService;
        _getCustomerService = getCustomerService;
        _updateCustomerService = updateCustomerService;
        _deleteCustomerService = deleteCustomerService;
    }

    [HttpPost]
    public async Task<IResult> CreateCustomer(CreateCustomerRequest request)
    {
        var result = await _createCustomerService.HandleAsync(request);
        return result.Execute();
    }

    [HttpGet("{id}")]
    public async Task<IResult> GetCustomer(int id)
    {
        var request = new GetCustomerRequest { CustomerId = id };
        var result = await _getCustomerService.HandleAsync(request);
        return result.Execute();
    }

    [HttpPut]
    public async Task<IResult> UpdateCustomer(UpdateCustomerRequest request)
    {
        var result = await _updateCustomerService.HandleAsync(request);
        return result.Execute();
    }

    [HttpDelete("{id}")]
    public async Task<IResult> DeleteCustomer(int id)
    {
        var request = new DeleteCustomerRequest { CustomerId = id };
        var result = await _deleteCustomerService.HandleAsync(request);
        return result.Execute();
    }
}