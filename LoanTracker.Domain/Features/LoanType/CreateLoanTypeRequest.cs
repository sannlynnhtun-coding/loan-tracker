namespace LoanTracker.Domain.Features.LoanType;

public class CreateLoanTypeRequest
{
    public string LoanTypeName { get; set; } = null!;
    public string? Description { get; set; }
}