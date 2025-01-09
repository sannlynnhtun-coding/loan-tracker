using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanTracker.Database.Models;

public class MortgageLoanModel
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string CustomerId { get; set; }
    public decimal LoanAmount { get; set; }
    public decimal InterestRate { get; set; }
    public int LoanTerm { get; set; }
    public DateTime StartDate { get; set; } = DateTime.Now;
    public decimal MonthlyPayment { get; set; }
    public decimal DownPayment { get; set; }
    public decimal TotalRepayment { get; set; }
}

public class LoanDTO
{
    public string CustomerId { get; set; }
    public decimal LoanAmount { get; set; }
    public decimal InterestRate { get; set; }
    public int LoanTerm { get; set; }
    public decimal DownPayment { get; set; }
}
