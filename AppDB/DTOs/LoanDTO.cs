using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDB.DTOs;

public class LoanDTO
{
    public Guid CustomerID { get; set; }
    public decimal LoanAmount { get; set; }
    public int LoanTerm { get; set; }
    public decimal DownPayment { get; set; }
}
