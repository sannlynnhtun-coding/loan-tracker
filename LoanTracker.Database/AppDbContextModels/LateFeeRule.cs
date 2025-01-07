using System;
using System.Collections.Generic;

namespace LoanTracker.Database.AppDbContextModels;

public partial class LateFeeRule
{
    public int RuleId { get; set; }

    public int MinDaysOverdue { get; set; }

    public int? MaxDaysOverdue { get; set; }

    public decimal LateFeeAmount { get; set; }
}
