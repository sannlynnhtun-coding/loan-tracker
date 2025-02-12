using System;
using System.Collections.Generic;

namespace LoanTracker.Database.AppDbContextModels;

public partial class TblLoanTypeBurmese
{
    public int LoanTypeId { get; set; }

    public string LoanTypeName { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime? CreatedDate { get; set; }

    public int? EnglishLoanTypeId { get; set; }

    public virtual TblLoanType? EnglishLoanType { get; set; }
}
