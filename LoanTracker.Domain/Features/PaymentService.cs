namespace LoanTracker.Domain.Features;

public class PaymentService(AppDbContext dbContext, LateFeeRuleService lateFeeRuleService)
{
    // Get all payments for a loan
    public async Task<Result<List<Payment>>> GetPaymentsByLoanIdAsync(int loanId)
    {
        var payments = await dbContext.Payments
            .Where(p => p.LoanId == loanId)
            .ToListAsync();

        return Result<List<Payment>>.Success(payments);
    }

    // Record a payment
    public async Task<Result<Payment>> RecordPaymentAsync(int loanId, DateOnly paymentDate, decimal amountPaid)
    {
        var loan = await dbContext.MortgageLoans.FindAsync(loanId);
        if (loan == null)
            return Result<Payment>.NotFoundError("Loan not found.");

        // Calculate late fee
        var lateFee = await CalculateLateFeeAsync(loanId, paymentDate);

        // Check if the payment exceeds the remaining repayment amount
        var totalPaid = await dbContext.Payments
            .Where(p => p.LoanId == loanId)
            .SumAsync(p => p.AmountPaid);

        if (totalPaid + amountPaid > loan.TotalRepayment)
            return Result<Payment>.ValidationError("Payment exceeds the total repayment amount.");

        // Create payment record
        var payment = new Payment
        {
            LoanId = loanId,
            PaymentDate = paymentDate,
            AmountPaid = amountPaid,
            LateFee = lateFee
        };

        dbContext.Payments.Add(payment);
        await dbContext.SaveChangesAsync();

        // Check if the loan is fully repaid
        if (totalPaid + amountPaid == loan.TotalRepayment)
            return Result<Payment>.Success(payment, "Loan fully repaid!");

        return Result<Payment>.Success(payment);
    }

    // Generate payment schedule
    public async Task<Result<List<PaymentSchedule>>> GeneratePaymentScheduleAsync(int loanId, string scheduleType)
    {
        var loan = await dbContext.MortgageLoans.FindAsync(loanId);
        if (loan == null)
            return Result<List<PaymentSchedule>>.NotFoundError("Loan not found.");

        var schedule = new List<PaymentSchedule>();
        decimal remainingBalance = loan.LoanAmount;
        DateOnly paymentDate = loan.StartDate;

        int totalPeriods = scheduleType.ToLower() == "yearly" ? loan.LoanTerm : loan.LoanTerm * 12;

        for (int period = 1; period <= totalPeriods; period++)
        {
            decimal interest = remainingBalance * (loan.InterestRate / 100) / (scheduleType.ToLower() == "yearly" ? 1 : 12);
            decimal principal = loan.MonthlyPayment.GetValueOrDefault() - interest;
            remainingBalance -= principal;

            schedule.Add(new PaymentSchedule
            {
                Period = period,
                PaymentDate = paymentDate,
                Principal = principal,
                Interest = interest,
                TotalPayment = loan.MonthlyPayment.GetValueOrDefault(),
                RemainingBalance = remainingBalance
            });

            paymentDate = scheduleType.ToLower() == "yearly" ? paymentDate.AddYears(1) : paymentDate.AddMonths(1);
        }

        return Result<List<PaymentSchedule>>.Success(schedule);
    }

    // Helper method to calculate late fee
    private async Task<decimal> CalculateLateFeeAsync(int loanId, DateOnly paymentDate)
    {
        var loan = await dbContext.MortgageLoans.FindAsync(loanId);
        if (loan == null)
            return 0;

        var dueDate = new DateOnly(paymentDate.Year, paymentDate.Month, 1); // Assuming due date is the 1st of the month
        var daysOverdue = paymentDate.DayNumber - dueDate.DayNumber;

        if (daysOverdue <= 0)
            return 0;

        var lateFeeRules = await lateFeeRuleService.GetAllLateFeeRuleAsync();
        if (!lateFeeRules.IsSuccess)
            return 0;

        var applicableRule = lateFeeRules.Data
            .FirstOrDefault(rule => daysOverdue >= rule.MinDaysOverdue && (daysOverdue <= rule.MaxDaysOverdue || rule.MaxDaysOverdue == null));

        return applicableRule?.LateFeeAmount ?? 0;
    }
}
