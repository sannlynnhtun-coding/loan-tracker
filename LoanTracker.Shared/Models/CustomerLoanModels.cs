using System.ComponentModel.DataAnnotations;

namespace LoanTracker.Shared.Models;

public class CustomerLoanRequest
{
    public int LoanId { get; set; }

    [Required]
    [Display(Name = "Customer")]
    public int CustomerId { get; set; }

    [Required]
    [Display(Name = "Loan Product")]
    public int LoanTypeId { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    [Display(Name = "Principal Amount")]
    public decimal PrincipalAmount { get; set; }

    [Required]
    [Range(0, 100)]
    [Display(Name = "Annual Interest Rate (%)")]
    public decimal InterestRate { get; set; }

    [Required]
    [Range(1, 120)]
    [Display(Name = "Loan Term (Months)")]
    public int LoanTerm { get; set; }

    [Required]
    [Display(Name = "Start Date")]
    [DataType(DataType.Date)]
    public DateTime LoanStartDate { get; set; }

    [Display(Name = "Repayment Frequency")]
    public string? RepaymentFrequency { get; set; }

    public string? Status { get; set; }
}

public class CustomerLoanResponse
{
    public int LoanId { get; set; }
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = null!;
    public int LoanTypeId { get; set; }
    public string LoanTypeName { get; set; } = null!;
    public decimal TotalAmount { get; set; }
    public decimal PrincipalAmount { get; set; }
    public decimal InterestRate { get; set; }
    public int LoanTerm { get; set; }
    public DateTime LoanStartDate { get; set; }
    public string? RepaymentFrequency { get; set; }
    public string? Status { get; set; }
    public string? StatusColor => Status switch
    {
        "Active" => "emerald",
        "Completed" => "blue",
        "Late" => "rose",
        _ => "slate"
    };
}
