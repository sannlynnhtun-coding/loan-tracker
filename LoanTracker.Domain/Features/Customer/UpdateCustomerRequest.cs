namespace LoanTracker.Domain.Features.Customer;

public class UpdateCustomerRequest
{
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = null!;
    public string Nrc { get; set; } = null!;
    public string MobileNo { get; set; } = null!;
    public string? Address { get; set; }
}