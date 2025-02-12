using System;
using System.Collections.Generic;

namespace LoanTracker.Database.AppDbContextModels;

public partial class VwCustomerLoan
{
    public int CustomerId { get; set; }

    public string CustomerName { get; set; } = null!;

    public string EnglishLoanType { get; set; } = null!;

    public string? BurmeseLoanType { get; set; }
}
