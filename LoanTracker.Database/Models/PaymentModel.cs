using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanTracker.Database.Models;

public class PaymentModel
{
    public string Id { get; set; }
    public string LoanId { get; set; }
    public DateTime PaymentDate { get; set; }
    public decimal AmountPaid { get; set; }
    public decimal LateFee { get; set; }
}
