namespace LoanTracker.Domain.Features.LoanType;

public class UpdateLoanTypeRequest
{
    public string LoanTypeName { get; set; } = null!;
    public string? Description { get; set; }
}