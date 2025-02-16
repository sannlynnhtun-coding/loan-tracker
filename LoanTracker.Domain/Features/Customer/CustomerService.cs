namespace LoanTracker.Application.Services;

public class CustomerService
{
    private readonly AppDbContext _context;

    public CustomerService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<TblCustomer>> AddCustomerAsync(TblCustomer customer)
    {
        if (await _context.TblCustomers.AnyAsync(c => c.Nrc == customer.Nrc))
            return Result<TblCustomer>.ValidationError("NRC already exists.");

        if (await _context.TblCustomers.AnyAsync(c => c.MobileNo == customer.MobileNo))
            return Result<TblCustomer>.ValidationError("Mobile number already exists.");

        customer.CreatedDate = DateTime.UtcNow;
        _context.TblCustomers.Add(customer);
        await _context.SaveChangesAsync();
        return Result<TblCustomer>.Success(customer, "Customer added successfully.");
    }

    public async Task<Result<TblCustomer>> GetCustomerByIdAsync(int customerId)
    {
        var customer = await _context.TblCustomers.FindAsync(customerId);
        if (customer == null)
            return Result<TblCustomer>.NotFoundError("Customer not found.");

        return Result<TblCustomer>.Success(customer, "Customer retrieved successfully.");
    }

    public async Task<Result<List<TblCustomer>>> SearchCustomersAsync(string searchTerm)
    {
        var customers = await _context.TblCustomers
            .Where(c => c.CustomerName.Contains(searchTerm) || c.Nrc.Contains(searchTerm))
            .ToListAsync();

        return Result<List<TblCustomer>>.Success(customers, "Customers retrieved successfully.");
    }

    public async Task<Result<List<TblCustomer>>> GetAllCustomersAsync()
    {
        var customers = await _context.TblCustomers.ToListAsync();
        return Result<List<TblCustomer>>.Success(customers, "Customers retrieved successfully.");
    }

    public async Task<Result<TblCustomer>> UpdateCustomerAsync(TblCustomer customer)
    {
        _context.TblCustomers.Update(customer);
        await _context.SaveChangesAsync();
        return Result<TblCustomer>.Success(customer, "Customer updated successfully.");
    }

    public async Task<Result<bool>> DeleteCustomerAsync(int customerId)
    {
        var customer = await _context.TblCustomers.FindAsync(customerId);
        if (customer == null)
            return Result<bool>.NotFoundError("Customer not found.");

        _context.TblCustomers.Remove(customer);
        await _context.SaveChangesAsync();
        return Result<bool>.Success(true, "Customer deleted successfully.");
    }
}
