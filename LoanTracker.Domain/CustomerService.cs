using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoanTracker.Database.AppDbContextModels;
using Microsoft.EntityFrameworkCore;

namespace LoanTracker.Domain;

public class CustomerService
{
    private readonly AppDbContext _dbContext;

    public CustomerService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    // Get all customers
    public async Task<Result<List<Customer>>> GetAllCustomersAsync()
    {
        var customers = await _dbContext.Customers.ToListAsync();
        return Result<List<Customer>>.Success(customers);
    }

    // Get customer by ID
    public async Task<Result<Customer>> GetCustomerByIdAsync(int id)
    {
        var customer = await _dbContext.Customers.FindAsync(id);
        if (customer == null)
            return Result<Customer>.NotFoundError();

        return Result<Customer>.Success(customer);
    }

    // Create a new customer
    public async Task<Result<Customer>> CreateCustomerAsync(Customer customer)
    {
        if (string.IsNullOrEmpty(customer.BorrowerName))
            return Result<Customer>.ValidationError("BorrowerName is required.");

        _dbContext.Customers.Add(customer);
        await _dbContext.SaveChangesAsync();

        return Result<Customer>.Success(customer);
    }

    // Update an existing customer
    public async Task<Result<Customer>> UpdateCustomerAsync(int id, Customer updatedCustomer)
    {
        var customer = await _dbContext.Customers.FindAsync(id);
        if (customer == null)
            return Result<Customer>.NotFoundError();

        customer.BorrowerName = updatedCustomer.BorrowerName;
        customer.Nrc = updatedCustomer.Nrc;

        await _dbContext.SaveChangesAsync();

        return Result<Customer>.Success(customer);
    }

    // Delete a customer
    public async Task<Result<Customer>> DeleteCustomerAsync(int id)
    {
        var customer = await _dbContext.Customers.FindAsync(id);
        if (customer == null)
            return Result<Customer>.NotFoundError();

        _dbContext.Customers.Remove(customer);
        await _dbContext.SaveChangesAsync();

        return Result<Customer>.Success(customer, "Customer deleted successfully.");
    }
}