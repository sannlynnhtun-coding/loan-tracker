namespace LoanTracker.Domain;

// PaymentSchedule DTO for generating payment schedules
public class PaymentSchedule
{
    public int Period { get; set; }
    public DateOnly PaymentDate { get; set; }
    public decimal Principal { get; set; }
    public decimal Interest { get; set; }
    public decimal TotalPayment { get; set; }
    public decimal RemainingBalance { get; set; }
}