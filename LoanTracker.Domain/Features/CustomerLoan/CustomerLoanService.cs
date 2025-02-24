namespace LoanTracker.Domain.Features.CustomerLoan;

public class CustomerLoanService
{
    private readonly AppDbContext _context;

    public CustomerLoanService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<TblCustomerLoan>> CreateLoanAsync(TblCustomerLoan loan)
    {
        // Validate loan type
        var loanType = await _context.TblLoanTypes.FindAsync(loan.LoanTypeId);
        if (loanType == null)
            return Result<TblCustomerLoan>.ValidationError("Loan type not found.");

        // Set loan status to "Active"
        loan.Status = "Active";

        // Add loan to the database
        _context.TblCustomerLoans.Add(loan);
        await _context.SaveChangesAsync();

        // Generate payment schedules
        await GeneratePaymentSchedulesAsync(loan);

        return Result<TblCustomerLoan>.Success(loan, "Loan created successfully.");
    }

    public async Task<Result<TblCustomerLoan>> GetLoanByIdAsync(int loanId)
    {
        var loan = await _context.TblCustomerLoans
            .Include(l => l.Customer)
            .Include(l => l.LoanType)
            .Include(l => l.TblPaymentSchedules)
            .FirstOrDefaultAsync(l => l.LoanId == loanId);

        if (loan == null)
            return Result<TblCustomerLoan>.NotFoundError("Loan not found.");

        return Result<TblCustomerLoan>.Success(loan, "Loan retrieved successfully.");
    }

    public async Task<Result<TblCustomerLoan>> UpdateLoanAsync(int loanId, TblCustomerLoan loan)
    {
        // Validate the loan exists
        var existingLoan = await _context.TblCustomerLoans
            .Include(l => l.TblPaymentSchedules)
            .FirstOrDefaultAsync(l => l.LoanId == loanId);

        if (existingLoan == null)
            return Result<TblCustomerLoan>.NotFoundError("Loan not found.");

        // Update the loan properties
        existingLoan.CustomerId = loan.CustomerId;
        existingLoan.LoanTypeId = loan.LoanTypeId;
        existingLoan.PrincipalAmount = loan.PrincipalAmount;
        existingLoan.InterestRate = loan.InterestRate;
        existingLoan.LoanTerm = loan.LoanTerm;
        existingLoan.LoanStartDate = loan.LoanStartDate;
        existingLoan.RepaymentFrequency = loan.RepaymentFrequency;
        existingLoan.Status = loan.Status;

        // Regenerate payment schedules if necessary
        await RegeneratePaymentSchedulesAsync(existingLoan);

        // Save changes to the database
        await _context.SaveChangesAsync();

        return Result<TblCustomerLoan>.Success(existingLoan, "Loan updated successfully.");
    }

    public async Task<Result<bool>> DeleteLoanAsync(int loanId)
    {
        var loan = await _context.TblCustomerLoans
            .Include(l => l.TblPaymentSchedules)
            .FirstOrDefaultAsync(l => l.LoanId == loanId);

        if (loan == null)
            return Result<bool>.NotFoundError("Loan not found.");

        // Remove associated payment schedules
        _context.TblPaymentSchedules.RemoveRange(loan.TblPaymentSchedules);

        // Remove the loan
        _context.TblCustomerLoans.Remove(loan);
        await _context.SaveChangesAsync();

        return Result<bool>.Success(true, "Loan deleted successfully.");
    }

    private async Task GeneratePaymentSchedulesAsync(TblCustomerLoan loan)
    {
        var paymentSchedules = new List<TblPaymentSchedule>();
        var startDate = loan.LoanStartDate;
        var principalAmount = loan.PrincipalAmount;
        var interestRate = loan.InterestRate;
        var loanTerm = loan.LoanTerm;
        var repaymentFrequency = loan.RepaymentFrequency;

        decimal monthlyInterestRate = interestRate / 100 / 12;
        int totalPayments = loanTerm * (repaymentFrequency == "Monthly" ? 12 : 1);

        // Calculate monthly payment using the loan amortization formula
        decimal monthlyPayment = principalAmount * (monthlyInterestRate / (1 - (decimal)Math.Pow(1 + (double)monthlyInterestRate, -totalPayments)));

        for (int i = 1; i <= totalPayments; i++)
        {
            var dueDate = startDate.AddMonths(i);
            var interestComponent = principalAmount * monthlyInterestRate;
            var principalComponent = monthlyPayment - interestComponent;
            principalAmount -= principalComponent;

            var schedule = new TblPaymentSchedule
            {
                LoanId = loan.LoanId,
                DueDate = dueDate,
                InstallmentAmount = monthlyPayment,
                PrincipalComponent = principalComponent,
                InterestComponent = interestComponent,
                RemainingBalance = principalAmount,
                Status = "Pending"
            };

            paymentSchedules.Add(schedule);
        }

        // Add payment schedules to the database
        _context.TblPaymentSchedules.AddRange(paymentSchedules);
        await _context.SaveChangesAsync();
    }

    private async Task RegeneratePaymentSchedulesAsync(TblCustomerLoan loan)
    {
        // Remove existing payment schedules
        var existingSchedules = await _context.TblPaymentSchedules
            .Where(s => s.LoanId == loan.LoanId)
            .ToListAsync();

        _context.TblPaymentSchedules.RemoveRange(existingSchedules);

        // Generate new payment schedules
        await GeneratePaymentSchedulesAsync(loan);
    }

    public async Task UpdatePaymentStatusAsync(int paymentId, decimal amountPaid, DateTime paymentDate)
    {
        var paymentSchedule = await _context.TblPaymentSchedules.FindAsync(paymentId);
        if (paymentSchedule == null)
            throw new Exception("Payment schedule not found.");

        // Check if payment is late
        if (DateOnly.FromDateTime(paymentDate) > paymentSchedule.DueDate)
        {
            int overdueDays = (paymentDate - Convert.ToDateTime(paymentSchedule.DueDate)).Days;
            decimal lateFee = paymentSchedule.InstallmentAmount * 0.01m * overdueDays;

            // Add late fee
            var lateFeeRecord = new TblLateFee
            {
                ScheduleId = paymentSchedule.ScheduleId,
                OverdueDays = overdueDays,
                LateFeeAmount = lateFee
            };

            _context.TblLateFees.Add(lateFeeRecord);
        }

        // Update payment status
        paymentSchedule.Status = amountPaid >= paymentSchedule.InstallmentAmount ? "Paid" : "Partial";

        // Check if all payments are completed
        var loan = await _context.TblCustomerLoans
            .Include(l => l.TblPaymentSchedules)
            .FirstOrDefaultAsync(l => l.LoanId == paymentSchedule.LoanId);

        if (loan != null && loan.TblPaymentSchedules.All(s => s.Status == "Paid"))
        {
            loan.Status = "Completed";
        }

        await _context.SaveChangesAsync();
    }
}