using Microsoft.Extensions.Logging;
using LoanTracker.Shared.Models;
using LoanTracker.Database.AppDbContextModels;
using Microsoft.EntityFrameworkCore;

namespace LoanTracker.Application.Services;

public class CustomerService
{
    private readonly AppDbContext _context;
    private readonly ILogger<CustomerService> _logger;

    public CustomerService(AppDbContext context, ILogger<CustomerService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<CustomerResponse>> AddCustomerAsync(CustomerRequest request)
    {
        try
        {
            _logger.LogInformation("Attempting to add customer: {CustomerName}", request.CustomerName);

            if (await _context.TblCustomers.AnyAsync(c => c.Nrc == request.Nrc))
            {
                _logger.LogWarning("Validation failed: NRC {Nrc} already exists.", request.Nrc);
                return Result<CustomerResponse>.ValidationError("NRC already exists.");
            }

            if (await _context.TblCustomers.AnyAsync(c => c.MobileNo == request.MobileNo))
            {
                _logger.LogWarning("Validation failed: Mobile number {MobileNo} already exists.", request.MobileNo);
                return Result<CustomerResponse>.ValidationError("Mobile number already exists.");
            }

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

            _logger.LogInformation("Customer added successfully with ID: {CustomerId}", customer.CustomerId);

            var response = MapToResponse(customer);
            return Result<CustomerResponse>.Success(response, "Customer added successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while adding customer: {CustomerName}", request.CustomerName);
            return Result<CustomerResponse>.Failure("An error occurred while adding the customer.");
        }
    }

    public async Task<Result<CustomerResponse>> GetCustomerByIdAsync(int customerId)
    {
        try
        {
            _logger.LogInformation("Fetching customer by ID: {CustomerId}", customerId);
            var customer = await _context.TblCustomers.FindAsync(customerId);
            if (customer == null)
            {
                _logger.LogWarning("Customer with ID {CustomerId} not found.", customerId);
                return Result<CustomerResponse>.NotFoundError("Customer not found.");
            }

            return Result<CustomerResponse>.Success(MapToResponse(customer), "Customer retrieved successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching customer by ID: {CustomerId}", customerId);
            return Result<CustomerResponse>.Failure("An error occurred while retrieving the customer.");
        }
    }

    public async Task<Result<List<CustomerResponse>>> SearchCustomersAsync(string searchTerm)
    {
        try
        {
            _logger.LogInformation("Searching customers with term: {SearchTerm}", searchTerm);
            var customers = await _context.TblCustomers
                .Where(c => c.CustomerName.Contains(searchTerm) || c.Nrc.Contains(searchTerm))
                .Select(c => MapToResponse(c))
                .ToListAsync();

            _logger.LogInformation("Found {Count} customers for search term: {SearchTerm}", customers.Count, searchTerm);
            return Result<List<CustomerResponse>>.Success(customers, "Customers retrieved successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while searching customers with term: {SearchTerm}", searchTerm);
            return Result<List<CustomerResponse>>.Failure("An error occurred while searching for customers.");
        }
    }

    public async Task<Result<List<CustomerResponse>>> GetAllCustomersAsync()
    {
        try
        {
            _logger.LogInformation("Fetching all customers.");
            var customers = await _context.TblCustomers
                .Select(c => MapToResponse(c))
                .ToListAsync();
            _logger.LogInformation("Retrieved {Count} customers.", customers.Count);
            return Result<List<CustomerResponse>>.Success(customers, "Customers retrieved successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching all customers.");
            return Result<List<CustomerResponse>>.Failure("An error occurred while retrieving all customers.");
        }
    }

    public async Task<Result<CustomerResponse>> UpdateCustomerAsync(CustomerRequest request)
    {
        try
        {
            _logger.LogInformation("Attempting to update customer with ID: {CustomerId}", request.CustomerId);
            
            var customer = await _context.TblCustomers.FindAsync(request.CustomerId);
            if (customer == null)
            {
                return Result<CustomerResponse>.NotFoundError("Customer not found.");
            }

            customer.CustomerName = request.CustomerName;
            customer.Nrc = request.Nrc;
            customer.MobileNo = request.MobileNo;
            customer.Address = request.Address;

            _context.TblCustomers.Update(customer);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Customer with ID {CustomerId} updated successfully.", customer.CustomerId);
            return Result<CustomerResponse>.Success(MapToResponse(customer), "Customer updated successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating customer with ID: {CustomerId}", request.CustomerId);
            return Result<CustomerResponse>.Failure("An error occurred while updating the customer.");
        }
    }

    public async Task<Result<bool>> DeleteCustomerAsync(int customerId)
    {
        try
        {
            _logger.LogInformation("Attempting to delete customer with ID: {CustomerId}", customerId);
            var customer = await _context.TblCustomers.FindAsync(customerId);
            if (customer == null)
            {
                _logger.LogWarning("Delete failed: Customer with ID {CustomerId} not found.", customerId);
                return Result<bool>.NotFoundError("Customer not found.");
            }

            _context.TblCustomers.Remove(customer);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Customer with ID {CustomerId} deleted successfully.", customerId);
            return Result<bool>.Success(true, "Customer deleted successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting customer with ID: {CustomerId}", customerId);
            return Result<bool>.Failure("An error occurred while deleting the customer.");
        }
    }

    private static CustomerResponse MapToResponse(TblCustomer customer)
    {
        return new CustomerResponse
        {
            CustomerId = customer.CustomerId,
            CustomerName = customer.CustomerName,
            Nrc = customer.Nrc,
            MobileNo = customer.MobileNo,
            Address = customer.Address,
            CreatedDate = customer.CreatedDate
        };
    }
}
