namespace LoanTracker.Domain.Features.LoanType;

public class LoanTypeResponse
{
    public int LoanTypeId { get; set; }
    public string LoanTypeName { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime? CreatedDate { get; set; }
}