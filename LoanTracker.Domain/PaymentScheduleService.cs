using LoanTracker.Database.AppDbContext;
using LoanTracker.Database.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanTracker.Domain;

public class PaymentScheduleService
{
    private readonly AppDbContext context;
    public PaymentScheduleService()
    {
        context = new AppDbContext();
    }
    public List<YearlyPaymentScheduleModel> GetYearlyPaymentSchedule(string loanId)
    {
        var loan = context.LoanDetails.FirstOrDefault(x => x.Id == loanId);
        var payments = context.Payment.Where(x => x.LoanId == loanId).ToList();
        var groupedPayments = payments
            .GroupBy(p => p.PaymentDate.Year)
            .Select(g => new
            {
                Year = g.Key,
                TotalPayment = g.Sum(p => p.AmountPaid),
                TotalLateFees = g.Sum(p => p.LateFee) 
            }).ToList();

        decimal remainingBalance = loan.TotalRepayment;

        List<YearlyPaymentScheduleModel> yearlySchedule = new List<YearlyPaymentScheduleModel>();
        foreach (var group in groupedPayments)
        {
            decimal interest = remainingBalance * (loan.InterestRate / 100);

            decimal principal = group.TotalPayment - interest;

            remainingBalance -= group.TotalPayment;

            yearlySchedule.Add(new YearlyPaymentScheduleModel
            {
                Year = group.Year,
                Principal = principal,
                Interest = interest,
                TotalPayment = group.TotalPayment,
                RemainingBalance = remainingBalance
            });
        }

        return yearlySchedule;
    }

}
