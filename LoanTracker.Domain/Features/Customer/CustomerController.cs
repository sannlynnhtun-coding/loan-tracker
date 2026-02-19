using LoanTracker.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace LoanTracker.Application.Services;

[ApiController]
[Route("api/[controller]")]
public class CustomerController : ControllerBase
{
    private readonly CustomerService _customerService;

    public CustomerController(CustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpPost]
    public async Task<IResult> AddCustomer([FromBody] CustomerRequest customer)
    {
        var result = await _customerService.AddCustomerAsync(customer);
        return result.Execute();
    }

    [HttpGet("{id}")]
    public async Task<IResult> GetCustomer(int id)
    {
        var result = await _customerService.GetCustomerByIdAsync(id);
        return result.Execute();
    }

    [HttpGet]
    public async Task<IResult> GetAllCustomers()
    {
        var result = await _customerService.GetAllCustomersAsync();
        return result.Execute();
    }

    [HttpPut("{id}")]
    public async Task<IResult> UpdateCustomer(int id, [FromBody] CustomerRequest customer)
    {
        customer.CustomerId = id;
        var result = await _customerService.UpdateCustomerAsync(customer);
        return result.Execute();
    }

    [HttpDelete("{id}")]
    public async Task<IResult> DeleteCustomer(int id)
    {
        var result = await _customerService.DeleteCustomerAsync(id);
        return result.Execute();
    }
}