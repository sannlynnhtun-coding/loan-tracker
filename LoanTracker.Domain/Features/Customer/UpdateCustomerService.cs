namespace LoanTracker.Domain.Features.Customer;

public class UpdateCustomerService
{
    private readonly AppDbContext _context;

    public UpdateCustomerService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<UpdateCustomerResponse>> HandleAsync(UpdateCustomerRequest request)
    {
        var existingCustomer = await _context.TblCustomers.FindAsync(request.CustomerId);

        if (existingCustomer == null)
            return Result<UpdateCustomerResponse>.NotFoundError("Customer not found.");

        existingCustomer.CustomerName = request.CustomerName;
        existingCustomer.Nrc = request.Nrc;
        existingCustomer.MobileNo = request.MobileNo;
        existingCustomer.Address = request.Address;

        await _context.SaveChangesAsync();

        var response = new UpdateCustomerResponse
        {
            CustomerId = existingCustomer.CustomerId,
            CustomerName = existingCustomer.CustomerName,
            Nrc = existingCustomer.Nrc,
            MobileNo = existingCustomer.MobileNo,
            Address = existingCustomer.Address,
            CreatedDate = existingCustomer.CreatedDate.ToDateTime()
        };

        return Result<UpdateCustomerResponse>.Success(response, "Customer updated successfully.");
    }
}