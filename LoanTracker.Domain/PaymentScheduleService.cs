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



    #region monthly payment shedule

    public List<MonthlyPaymentScheduleModel> GetMonthlyPaymentSchedules(string loanId)
    {
        var loan = context.LoanDetails.FirstOrDefault(x => x.Id == loanId);

        if (loanId is null)
        {
            return null;
        }

        var payments = context.Payment.Where(x => x.LoanId == loanId).ToList();
        var pymentsGroup = payments.GroupBy(x => new { x.PaymentDate.Month })
            .Select(x => new
            {
                Month = x.Key.Month,
                TotalPayment = x.Sum(x => x.AmountPaid),
                TotalLateFees = x.Sum(x => x.LateFee)

            }).ToList();

        decimal remainingBalance = loan.TotalRepayment;
        decimal monthlyInterestRate = (loan.InterestRate / 100) / 12;

        List<MonthlyPaymentScheduleModel> monthlySchedule = new List<MonthlyPaymentScheduleModel>();

        foreach (var group in pymentsGroup)
        {
            decimal interest = remainingBalance * monthlyInterestRate;
            decimal principal = group.TotalPayment - interest;
            remainingBalance -= principal;

            monthlySchedule.Add(new MonthlyPaymentScheduleModel
            {

                Month = group.Month,
                Principal = principal,
                Interest = interest,
                TotalPayment = group.TotalPayment,
                RemainingBalance = remainingBalance
            });
        }

        return monthlySchedule;
    }
    #endregion

}
