using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDB.Model;

public class LoanDetails
{
    public Guid LoanID { get; set; } = Guid.NewGuid();
    public Guid CustomerID { get; set; }
    public decimal LoanAmount { get; set; }
    public int LoanTerm { get; set; }
    public DateTime StartDate { get; set; }
    public decimal MonthlyPayment { get; set; }
    public decimal DownPayment { get; set; }
    public decimal TotalRepaymentAmount { get; set; }
    public bool CompleteStatus { get; set; }
}
