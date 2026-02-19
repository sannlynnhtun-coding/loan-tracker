using System.ComponentModel.DataAnnotations;

namespace LoanTracker.Shared.Models;

public class CustomerPaymentRequest
{
    public int PaymentId { get; set; }

    [Required]
    public int ScheduleId { get; set; }

    [Required]
    [DataType(DataType.Date)]
    public DateTime PaymentDate { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    public decimal AmountPaid { get; set; }

    public decimal? LateFee { get; set; }
    public string? Status { get; set; }
}

public class CustomerPaymentResponse
{
    public int PaymentId { get; set; }
    public int ScheduleId { get; set; }
    public DateTime PaymentDate { get; set; }
    public decimal AmountPaid { get; set; }
    public decimal? LateFee { get; set; }
    public string? Status { get; set; }
    public string? StatusColor => Status switch
    {
        "On-Time" => "emerald",
        "Late" => "rose",
        _ => "slate"
    };
}
