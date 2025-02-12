namespace LoanTracker.Domain.Features.Customer;

public class DeleteCustomerResponse
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = null!;
}