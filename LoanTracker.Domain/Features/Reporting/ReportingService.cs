using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanTracker.Domain.Features.Reporting;

public class ReportingService
{
    private readonly AppDbContext _context;

    public ReportingService(AppDbContext context)
    {
        _context = context;
    }

    // Generate loan report
    public async Task<Result<LoanReport>> GenerateLoanReportAsync()
    {
        var activeLoans = await _context.TblCustomerLoans
            .Where(l => l.Status == "Active")
            .ToListAsync();

        var completedLoans = await _context.TblCustomerLoans
            .Where(l => l.Status == "Completed")
            .ToListAsync();

        var defaultedLoans = await _context.TblCustomerLoans
            .Where(l => l.Status == "Defaulted")
            .ToListAsync();

        var overduePayments = await _context.TblCustomerPayments
            .Where(cp => cp.Status == "Late")
            .ToListAsync();

        var report = new LoanReport
        {
            ActiveLoans = activeLoans,
            CompletedLoans = completedLoans,
            DefaultedLoans = defaultedLoans,
            OverduePayments = overduePayments,
            TotalLateFeesCollected = overduePayments.Sum(cp => cp.LateFee ?? 0)
        };

        return Result<LoanReport>.Success(report, "Loan report generated successfully.");
    }
}

public class LoanReport
{
    public List<TblCustomerLoan> ActiveLoans { get; set; } = new List<TblCustomerLoan>();

    public List<TblCustomerLoan> CompletedLoans { get; set; } = new List<TblCustomerLoan>();

    public List<TblCustomerLoan> DefaultedLoans { get; set; } = new List<TblCustomerLoan>();

    public List<TblCustomerPayment> OverduePayments { get; set; } = new List<TblCustomerPayment>();

    public decimal TotalLateFeesCollected { get; set; }
}