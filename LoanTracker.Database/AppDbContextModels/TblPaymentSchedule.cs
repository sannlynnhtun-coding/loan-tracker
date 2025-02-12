using System;
using System.Collections.Generic;

namespace LoanTracker.Database.AppDbContextModels;

public partial class TblPaymentSchedule
{
    public int ScheduleId { get; set; }

    public int LoanId { get; set; }

    public DateOnly DueDate { get; set; }

    public decimal InstallmentAmount { get; set; }

    public decimal PrincipalComponent { get; set; }

    public decimal InterestComponent { get; set; }

    public decimal RemainingBalance { get; set; }

    public string? Status { get; set; }

    public virtual TblCustomerLoan Loan { get; set; } = null!;

    public virtual ICollection<TblCustomerPayment> TblCustomerPayments { get; set; } = new List<TblCustomerPayment>();

    public virtual ICollection<TblLateFee> TblLateFees { get; set; } = new List<TblLateFee>();
}
