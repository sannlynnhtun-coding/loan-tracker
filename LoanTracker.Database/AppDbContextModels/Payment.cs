using System;
using System.Collections.Generic;

namespace LoanTracker.Database.AppDbContextModels;

public partial class Payment
{
    public int PaymentId { get; set; }

    public int LoanId { get; set; }

    public DateOnly PaymentDate { get; set; }

    public decimal AmountPaid { get; set; }

    public decimal? LateFee { get; set; }

    public virtual MortgageLoan Loan { get; set; } = null!;
}
