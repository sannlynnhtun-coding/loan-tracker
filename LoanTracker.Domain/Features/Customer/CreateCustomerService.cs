namespace LoanTracker.Domain.Features.Customer;

public class CreateCustomerService
{
    private readonly AppDbContext _context;

    public CreateCustomerService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<CreateCustomerResponse>> HandleAsync(CreateCustomerRequest request)
    {
        if (await _context.TblCustomers.AnyAsync(c => c.Nrc == request.Nrc))
            return Result<CreateCustomerResponse>.ValidationError("Customer with the same NRC already exists.");

        var customer = new TblCustomer
        {
            CustomerName = request.CustomerName,
            Nrc = request.Nrc,
            MobileNo = request.MobileNo,
            Address = request.Address,
            CreatedDate = DateTime.UtcNow
        };

        _context.TblCustomers.Add(customer);
        await _context.SaveChangesAsync();

        var response = new CreateCustomerResponse
        {
            CustomerId = customer.CustomerId,
            CustomerName = customer.CustomerName,
            Nrc = customer.Nrc,
            MobileNo = customer.MobileNo,
            Address = customer.Address,
            CreatedDate = customer.CreatedDate.ToDateTime()
        };

        return Result<CreateCustomerResponse>.Success(response, "Customer created successfully.");
    }
}