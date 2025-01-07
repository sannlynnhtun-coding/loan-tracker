using System;
using System.Collections.Generic;

namespace LoanTracker.Database.AppDbContextModels;

public partial class Customer
{
    public int CustomerId { get; set; }

    public string BorrowerName { get; set; } = null!;

    public virtual ICollection<MortgageLoan> MortgageLoans { get; set; } = new List<MortgageLoan>();
}
