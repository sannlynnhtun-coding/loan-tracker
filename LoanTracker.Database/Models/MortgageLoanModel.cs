using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace loan_tracker.Database.Models;

public class MortgageLoanModel
{
	public string Id { get; set; }
	public string CustomerId { get; set; }
	public decimal LoanAmount { get; set; }
	public decimal InterestRate { get; set; }
	public int LoanTerm { get; set; }
	public DateTime StartDate { get; set; }
	public decimal MonthlyPayment { get; set; }
	public decimal DownPayment { get; set; }
	public decimal TotalRepayment { get; set; }
}
