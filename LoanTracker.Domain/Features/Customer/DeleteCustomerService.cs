namespace LoanTracker.Domain.Features.Customer;

public class DeleteCustomerService
{
    private readonly AppDbContext _context;

    public DeleteCustomerService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<DeleteCustomerResponse>> HandleAsync(DeleteCustomerRequest request)
    {
        var customer = await _context.TblCustomers.FindAsync(request.CustomerId);

        if (customer == null)
            return Result<DeleteCustomerResponse>.NotFoundError("Customer not found.");

        _context.TblCustomers.Remove(customer);
        await _context.SaveChangesAsync();

        var response = new DeleteCustomerResponse
        {
            IsSuccess = true,
            Message = "Customer deleted successfully."
        };

        return Result<DeleteCustomerResponse>.Success(response, response.Message);
    }
}