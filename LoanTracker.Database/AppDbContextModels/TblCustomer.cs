using System;
using System.Collections.Generic;

namespace LoanTracker.Database.AppDbContextModels;

public partial class TblCustomer
{
    public int CustomerId { get; set; }

    public string CustomerName { get; set; } = null!;

    public string Nrc { get; set; } = null!;

    public string MobileNo { get; set; } = null!;

    public string? Address { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual ICollection<TblCustomerLoan> TblCustomerLoans { get; set; } = new List<TblCustomerLoan>();
}
