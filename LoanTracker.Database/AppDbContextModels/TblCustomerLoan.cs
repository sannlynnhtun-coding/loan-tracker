using System;
using System.Collections.Generic;

namespace LoanTracker.Database.AppDbContextModels;

public partial class TblCustomerLoan
{
    public int LoanId { get; set; }

    public int CustomerId { get; set; }

    public int LoanTypeId { get; set; }

    public decimal PrincipalAmount { get; set; }

    public decimal InterestRate { get; set; }

    public int LoanTerm { get; set; }

    public DateOnly LoanStartDate { get; set; }

    public string? RepaymentFrequency { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual TblCustomer Customer { get; set; } = null!;

    public virtual TblLoanType LoanType { get; set; } = null!;

    public virtual ICollection<TblPaymentSchedule> TblPaymentSchedules { get; set; } = new List<TblPaymentSchedule>();
}
