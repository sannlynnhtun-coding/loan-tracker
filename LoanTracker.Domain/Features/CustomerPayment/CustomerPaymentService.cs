using Microsoft.Extensions.Logging;
using LoanTracker.Shared.Models;
using LoanTracker.Database.AppDbContextModels;
using Microsoft.EntityFrameworkCore;

namespace LoanTracker.Domain.Features.CustomerPayment;

public class CustomerPaymentService
{
    private readonly AppDbContext _context;
    private readonly ILogger<CustomerPaymentService> _logger;

    public CustomerPaymentService(AppDbContext context, ILogger<CustomerPaymentService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<CustomerPaymentResponse>> AddPaymentAsync(CustomerPaymentRequest request)
    {
        try
        {
            _logger.LogInformation("Attempting to add payment for Schedule ID: {ScheduleId}", request.ScheduleId);

            var schedule = await _context.TblPaymentSchedules
                .Include(ps => ps.Loan)
                .FirstOrDefaultAsync(ps => ps.ScheduleId == request.ScheduleId);

            if (schedule == null)
            {
                _logger.LogWarning("Payment addition failed: Payment schedule with ID {ScheduleId} not found.", request.ScheduleId);
                return Result<CustomerPaymentResponse>.NotFoundError("Payment schedule not found.");
            }

            var payment = new TblCustomerPayment
            {
                ScheduleId = request.ScheduleId,
                AmountPaid = request.AmountPaid,
                PaymentDate = DateOnly.FromDateTime(request.PaymentDate),
                Status = "On-Time"
            };

            var dueDateAsDateTime = schedule.DueDate.ToDateTime(TimeOnly.MinValue);

            if (request.PaymentDate.Date > dueDateAsDateTime.Date)
            {
                payment.Status = "Late";

                int overdueDays = (request.PaymentDate.Date - dueDateAsDateTime.Date).Days;
                decimal lateFeeAmount = overdueDays * 0.01m * schedule.InstallmentAmount; // 1% per day

                _logger.LogInformation("Late payment detected for Schedule ID: {ScheduleId}. Overdue days: {OverdueDays}, Late fee: {LateFee}", request.ScheduleId, overdueDays, lateFeeAmount);

                payment.LateFee = lateFeeAmount;

                var lateFee = new TblLateFee
                {
                    ScheduleId = request.ScheduleId,
                    OverdueDays = overdueDays,
                    LateFeeAmount = lateFeeAmount
                };

                _context.TblLateFees.Add(lateFee);
            }

            _context.TblCustomerPayments.Add(payment);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Payment added successfully for Schedule ID: {ScheduleId}. Payment ID: {PaymentId}", payment.ScheduleId, payment.PaymentId);
            return Result<CustomerPaymentResponse>.Success(MapToResponse(payment), "Payment added successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while adding payment for Schedule ID: {ScheduleId}", request.ScheduleId);
            return Result<CustomerPaymentResponse>.Failure("An error occurred while adding the payment.");
        }
    }

    public async Task<Result<CustomerPaymentResponse>> GetPaymentByIdAsync(int paymentId)
    {
        try
        {
            _logger.LogInformation("Fetching payment by ID: {PaymentId}", paymentId);
            var payment = await _context.TblCustomerPayments.FindAsync(paymentId);
            if (payment == null)
            {
                _logger.LogWarning("Payment with ID {PaymentId} not found.", paymentId);
                return Result<CustomerPaymentResponse>.NotFoundError("Payment not found.");
            }

            return Result<CustomerPaymentResponse>.Success(MapToResponse(payment), "Payment retrieved successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching payment by ID: {PaymentId}", paymentId);
            return Result<CustomerPaymentResponse>.Failure("An error occurred while retrieving the payment.");
        }
    }

    private static CustomerPaymentResponse MapToResponse(TblCustomerPayment payment)
    {
        return new CustomerPaymentResponse
        {
            PaymentId = payment.PaymentId,
            ScheduleId = payment.ScheduleId,
            PaymentDate = payment.PaymentDate.ToDateTime(TimeOnly.MinValue),
            AmountPaid = payment.AmountPaid,
            LateFee = payment.LateFee,
            Status = payment.Status
        };
    }
}
