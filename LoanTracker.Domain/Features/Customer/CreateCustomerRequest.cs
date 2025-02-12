namespace LoanTracker.Domain.Features.Customer;

public class CreateCustomerRequest
{
    public string CustomerName { get; set; } = null!;
    public string Nrc { get; set; } = null!;
    public string MobileNo { get; set; } = null!;
    public string? Address { get; set; }
}