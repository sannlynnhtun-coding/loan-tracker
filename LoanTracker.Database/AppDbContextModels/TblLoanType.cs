using System;
using System.Collections.Generic;

namespace LoanTracker.Database.AppDbContextModels;

public partial class TblLoanType
{
    public int LoanTypeId { get; set; }

    public string LoanTypeName { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual ICollection<TblCustomerLoan> TblCustomerLoans { get; set; } = new List<TblCustomerLoan>();

    public virtual ICollection<TblLoanTypeBurmese> TblLoanTypeBurmeses { get; set; } = new List<TblLoanTypeBurmese>();
}
