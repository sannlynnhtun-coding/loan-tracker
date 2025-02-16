namespace LoanTracker.Domain.Features.CustomerPayment;

public class CustomerPaymentService
{
    private readonly AppDbContext _context;

    public CustomerPaymentService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<TblCustomerPayment>> AddPaymentAsync(TblCustomerPayment payment)
    {
        var schedule = await _context.TblPaymentSchedules.FindAsync(payment.ScheduleId);
        if (schedule == null)
            return Result<TblCustomerPayment>.NotFoundError("Payment schedule not found.");

        var dueDateAsDateTime = schedule.DueDate.ToDateTime(TimeOnly.MinValue);

        if (payment.PaymentDate > DateOnly.FromDateTime(dueDateAsDateTime))
        {
            payment.Status = "Late";
            int overdueDays = (Convert.ToDateTime(payment.PaymentDate) - dueDateAsDateTime).Days;
            payment.LateFee = overdueDays * 0.01m * schedule.InstallmentAmount; // 1% per day
        }
        else
        {
            payment.Status = "On-Time";
        }

        _context.TblCustomerPayments.Add(payment);
        await _context.SaveChangesAsync();
        return Result<TblCustomerPayment>.Success(payment, "Payment added successfully.");
    }

    public async Task<Result<TblCustomerPayment>> GetPaymentByIdAsync(int paymentId)
    {
        var payment = await _context.TblCustomerPayments.FindAsync(paymentId);
        if (payment == null)
            return Result<TblCustomerPayment>.NotFoundError("Payment not found.");

        return Result<TblCustomerPayment>.Success(payment, "Payment retrieved successfully.");
    }
}
