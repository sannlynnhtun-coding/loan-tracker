using System;
using System.Collections.Generic;

namespace LoanTracker.Database.AppDbContextModels;

public partial class MortgageLoan
{
    public int LoanId { get; set; }

    public int CustomerId { get; set; }

    public decimal LoanAmount { get; set; }

    public decimal InterestRate { get; set; }

    public int LoanTerm { get; set; }

    public DateOnly StartDate { get; set; }

    public decimal? MonthlyPayment { get; set; }

    public decimal? DownPayment { get; set; }

    public decimal? TotalRepayment { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
