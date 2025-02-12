namespace LoanTracker.Domain.Features.Customer;

public class GetCustomerService
{
    private readonly AppDbContext _context;

    public GetCustomerService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<GetCustomerResponse>> HandleAsync(GetCustomerRequest request)
    {
        var customer = await _context.TblCustomers.FindAsync(request.CustomerId);

        if (customer == null)
            return Result<GetCustomerResponse>.NotFoundError("Customer not found.");

        var response = new GetCustomerResponse
        {
            CustomerId = customer.CustomerId,
            CustomerName = customer.CustomerName,
            Nrc = customer.Nrc,
            MobileNo = customer.MobileNo,
            Address = customer.Address,
            CreatedDate = customer.CreatedDate.ToDateTime()
        };

        return Result<GetCustomerResponse>.Success(response, "Customer retrieved successfully.");
    }
}