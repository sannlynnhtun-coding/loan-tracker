namespace LoanTracker.Domain.Features;

public class CustomerService(AppDbContext dbContext)
{
    // Get all customers
    public async Task<Result<List<Customer>>> GetAllCustomersAsync()
    {
        var customers = await dbContext.Customers.ToListAsync();
        return Result<List<Customer>>.Success(customers);
    }

    // Get customer by ID
    public async Task<Result<Customer>> GetCustomerByIdAsync(int id)
    {
        var customer = await dbContext.Customers.FindAsync(id);
        if (customer == null)
            return Result<Customer>.NotFoundError();

        return Result<Customer>.Success(customer);
    }

    // Create a new customer
    public async Task<Result<Customer>> CreateCustomerAsync(Customer customer)
    {
        if (string.IsNullOrEmpty(customer.BorrowerName))
            return Result<Customer>.ValidationError("BorrowerName is required.");

        dbContext.Customers.Add(customer);
        await dbContext.SaveChangesAsync();

        return Result<Customer>.Success(customer);
    }

    // Update an existing customer
    public async Task<Result<Customer>> UpdateCustomerAsync(int id, Customer updatedCustomer)
    {
        var customer = await dbContext.Customers.FindAsync(id);
        if (customer == null)
            return Result<Customer>.NotFoundError();

        customer.BorrowerName = updatedCustomer.BorrowerName;
        customer.Nrc = updatedCustomer.Nrc;

        await dbContext.SaveChangesAsync();

        return Result<Customer>.Success(customer);
    }

    // Delete a customer
    public async Task<Result<Customer>> DeleteCustomerAsync(int id)
    {
        var customer = await dbContext.Customers.FindAsync(id);
        if (customer == null)
            return Result<Customer>.NotFoundError();

        dbContext.Customers.Remove(customer);
        await dbContext.SaveChangesAsync();

        return Result<Customer>.Success(customer, "Customer deleted successfully.");
    }
}