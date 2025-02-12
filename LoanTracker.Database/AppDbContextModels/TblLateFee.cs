using System;
using System.Collections.Generic;

namespace LoanTracker.Database.AppDbContextModels;

public partial class TblLateFee
{
    public int LateFeeId { get; set; }

    public int ScheduleId { get; set; }

    public int OverdueDays { get; set; }

    public decimal LateFeeAmount { get; set; }

    public virtual TblPaymentSchedule Schedule { get; set; } = null!;
}
