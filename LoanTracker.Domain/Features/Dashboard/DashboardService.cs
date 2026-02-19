using Microsoft.EntityFrameworkCore;
using LoanTracker.Shared.Models;
using LoanTracker.Database.AppDbContextModels;

namespace LoanTracker.Application.Services;

public class DashboardService
{
    private readonly AppDbContext _context;

    public DashboardService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<DashboardResponse>> GetDashboardDataAsync()
    {
        try
        {
            var today = DateTime.Today;
            var todayDateOnly = DateOnly.FromDateTime(today);
            var response = new DashboardResponse
            {
                TotalPortfolio = await _context.TblCustomerLoans
                    .Where(l => l.Status == "Active")
                    .SumAsync(l => (decimal?)l.PrincipalAmount) ?? 0,
                
                ActiveLoansCount = await _context.TblCustomerLoans
                    .CountAsync(l => l.Status == "Active"),
                
                LatePaymentsCount = await _context.TblPaymentSchedules
                    .CountAsync(s => s.Status == "Pending" && s.DueDate < todayDateOnly),
                
                NewRequestsCount = await _context.TblCustomerLoans
                    .CountAsync(l => l.CreatedDate >= today)
            };

            // Recent Payments
            var recentPayments = await _context.TblCustomerPayments
                .Include(p => p.Schedule)
                .ThenInclude(s => s.Loan)
                .ThenInclude(l => l.Customer)
                .OrderByDescending(p => p.PaymentId)
                .Take(5)
                .ToListAsync();

            foreach (var p in recentPayments)
            {
                response.RecentActivities.Add(new RecentActivityResponse
                {
                    Title = "Payment Received",
                    Description = $"Customer {p.Schedule.Loan.Customer.CustomerName} paid {p.AmountPaid:N0} for Loan #{p.Schedule.LoanId}.",
                    TimeAgo = "Just now",
                    Type = ActivityType.PaymentReceived
                });
            }

            // Recent Loans
            var recentLoans = await _context.TblCustomerLoans
                .Include(l => l.Customer)
                .OrderByDescending(l => l.LoanId)
                .Take(5)
                .ToListAsync();

            foreach (var l in recentLoans)
            {
                response.RecentActivities.Add(new RecentActivityResponse
                {
                    Title = "New Loan Issued",
                    Description = $"New loan of {l.PrincipalAmount:N0} approved for {l.Customer.CustomerName}.",
                    TimeAgo = "Recently",
                    Type = ActivityType.LoanCreated
                });
            }

            return Result<DashboardResponse>.Success(response);
        }
        catch (Exception ex)
        {
            return Result<DashboardResponse>.Failure($"Failed to load dashboard: {ex.Message}");
        }
    }
}
