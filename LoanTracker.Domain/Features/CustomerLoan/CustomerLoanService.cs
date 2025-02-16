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
        var loanType = await _context.TblLoanTypes.FindAsync(loan.LoanTypeId);
        if (loanType == null)
            return Result<TblCustomerLoan>.ValidationError("Loan type not found.");

        loan.Status = "Active";

        _context.TblCustomerLoans.Add(loan);
        await _context.SaveChangesAsync();
        return Result<TblCustomerLoan>.Success(loan, "Loan created successfully.");
    }

    public async Task<Result<TblCustomerLoan>> GetLoanByIdAsync(int loanId)
    {
        var loan = await _context.TblCustomerLoans
            .Include(l => l.Customer)
            .Include(l => l.LoanType)
            .FirstOrDefaultAsync(l => l.LoanId == loanId);

        if (loan == null)
            return Result<TblCustomerLoan>.NotFoundError("Loan not found.");

        return Result<TblCustomerLoan>.Success(loan, "Loan retrieved successfully.");
    }

    public async Task<Result<TblCustomerLoan>> UpdateLoanAsync(int loanId, TblCustomerLoan loan)
    {
        // Validate the loan exists
        var existingLoan = await _context.TblCustomerLoans.FindAsync(loanId);
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

        // Save changes to the database
        await _context.SaveChangesAsync();

        return Result<TblCustomerLoan>.Success(existingLoan, "Loan updated successfully.");
    }

    public async Task<Result<bool>> DeleteLoanAsync(int loanId)
    {
        var loan = await _context.TblCustomerLoans.FindAsync(loanId);
        if (loan == null)
            return Result<bool>.NotFoundError("Loan not found.");

        _context.TblCustomerLoans.Remove(loan);
        await _context.SaveChangesAsync();
        return Result<bool>.Success(true, "Loan deleted successfully.");
    }
}
