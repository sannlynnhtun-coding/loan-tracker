using System;
using System.Collections.Generic;

namespace LoanTracker.Database.AppDbContextModels;

public partial class VwLoanType
{
    public int EnglishLoanTypeId { get; set; }

    public string EnglishLoanTypeName { get; set; } = null!;

    public string BurmeseLoanTypeName { get; set; } = null!;

    public string? EnglishDescription { get; set; }

    public string? BurmeseDescription { get; set; }
}
