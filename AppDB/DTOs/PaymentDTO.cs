using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDB.DTOs;

public class PaymentDTO
{
    public Guid LoanID { get; set; }
    public Guid CustomerID { get; set; }
    public DateTime PaymentDate { get; set; }
    public decimal PaidAmount { get; set; }
}
