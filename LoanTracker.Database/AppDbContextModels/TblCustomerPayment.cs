using System;
using System.Collections.Generic;

namespace LoanTracker.Database.AppDbContextModels;

public partial class TblCustomerPayment
{
    public int PaymentId { get; set; }

    public int ScheduleId { get; set; }

    public DateOnly PaymentDate { get; set; }

    public decimal AmountPaid { get; set; }

    public decimal? LateFee { get; set; }

    public string? Status { get; set; }

    public virtual TblPaymentSchedule Schedule { get; set; } = null!;
}
