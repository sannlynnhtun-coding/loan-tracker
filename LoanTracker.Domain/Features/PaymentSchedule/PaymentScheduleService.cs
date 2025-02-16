namespace LoanTracker.Domain.Features.PaymentSchedule;

public class PaymentScheduleService
{
    private readonly AppDbContext _context;

    public PaymentScheduleService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<TblPaymentSchedule>>> GeneratePaymentScheduleAsync(int loanId)
    {
        var loan = await _context.TblCustomerLoans.FindAsync(loanId);
        if (loan == null)
            return Result<List<TblPaymentSchedule>>.NotFoundError("Loan not found.");

        var schedules = new List<TblPaymentSchedule>();
        decimal remainingBalance = loan.PrincipalAmount;
        decimal installmentAmount = loan.PrincipalAmount / loan.LoanTerm;

        for (int i = 0; i < loan.LoanTerm; i++)
        {
            var schedule = new TblPaymentSchedule
            {
                LoanId = loanId,
                DueDate = loan.LoanStartDate.AddMonths(i + 1),
                InstallmentAmount = installmentAmount,
                PrincipalComponent = installmentAmount * (1 - loan.InterestRate / 100),
                InterestComponent = installmentAmount * (loan.InterestRate / 100),
                RemainingBalance = remainingBalance - installmentAmount,
                Status = "Pending"
            };

            schedules.Add(schedule);
            remainingBalance -= installmentAmount;
        }

        _context.TblPaymentSchedules.AddRange(schedules);
        await _context.SaveChangesAsync();
        return Result<List<TblPaymentSchedule>>.Success(schedules, "Payment schedules generated successfully.");
    }

    public async Task<Result<List<TblPaymentSchedule>>> GetUpcomingPaymentsAsync(int loanId)
    {
        var schedules = await _context.TblPaymentSchedules
            .Where(s => s.LoanId == loanId && s.DueDate >= DateOnly.FromDateTime(DateTime.UtcNow))
            .ToListAsync();

        return Result<List<TblPaymentSchedule>>.Success(schedules, "Upcoming payments retrieved successfully.");
    }

    public async Task<Result<TblPaymentSchedule>> MarkPaymentAsCompletedAsync(int scheduleId)
    {
        var schedule = await _context.TblPaymentSchedules.FindAsync(scheduleId);
        if (schedule == null)
            return Result<TblPaymentSchedule>.NotFoundError("Payment schedule not found.");

        schedule.Status = "Paid";
        _context.TblPaymentSchedules.Update(schedule);
        await _context.SaveChangesAsync();
        return Result<TblPaymentSchedule>.Success(schedule, "Payment marked as completed.");
    }
}