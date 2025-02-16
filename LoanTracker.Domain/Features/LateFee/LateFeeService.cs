namespace LoanTracker.Domain.Features.LateFee;

public class LateFeeService
{
    private readonly AppDbContext _context;

    public LateFeeService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<TblLateFee>> AddLateFeeAsync(TblLateFee lateFee)
    {
        var schedule = await _context.TblPaymentSchedules.FindAsync(lateFee.ScheduleId);
        if (schedule == null)
            return Result<TblLateFee>.NotFoundError("Payment schedule not found.");

        _context.TblLateFees.Add(lateFee);
        await _context.SaveChangesAsync();
        return Result<TblLateFee>.Success(lateFee, "Late fee added successfully.");
    }

    public async Task<Result<TblLateFee>> GetLateFeeByIdAsync(int lateFeeId)
    {
        var lateFee = await _context.TblLateFees.FindAsync(lateFeeId);
        if (lateFee == null)
            return Result<TblLateFee>.NotFoundError("Late fee not found.");

        return Result<TblLateFee>.Success(lateFee, "Late fee retrieved successfully.");
    }

    public async Task<Result<decimal>> GetOverdueBalanceAsync(int customerId)
    {
        var overduePayments = await _context.TblCustomerPayments
            .Where(cp => cp.Schedule.Loan.CustomerId == customerId && cp.Status == "Late")
            .ToListAsync();

        decimal overdueBalance = overduePayments.Sum(cp => cp.LateFee ?? 0);
        return Result<decimal>.Success(overdueBalance, "Overdue balance retrieved successfully.");
    }
}