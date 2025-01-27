namespace LoanTracker.Domain.Features;

public class MortgageLoanService(AppDbContext dbContext)
{
    // Get all mortgage loans
    public async Task<Result<List<MortgageLoan>>> GetAllMortgageLoansAsync()
    {
        var loans = await dbContext.MortgageLoans.ToListAsync();
        return Result<List<MortgageLoan>>.Success(loans);
    }

    // Get mortgage loan by ID
    public async Task<Result<MortgageLoan>> GetMortgageLoanByIdAsync(int id)
    {
        var loan = await dbContext.MortgageLoans.FindAsync(id);
        if (loan == null)
            return Result<MortgageLoan>.NotFoundError();

        return Result<MortgageLoan>.Success(loan);
    }

    // Create a new mortgage loan
    public async Task<Result<MortgageLoan>> CreateMortgageLoanAsync(MortgageLoan loan)
    {
        // Validate input
        if (loan.LoanAmount <= 0 || loan.InterestRate <= 0 || loan.LoanTerm <= 0)
            return Result<MortgageLoan>.ValidationError("LoanAmount, InterestRate, and LoanTerm must be greater than 0.");

        // Calculate monthly payment and total repayment
        loan.MonthlyPayment = CalculateMonthlyPayment(loan.LoanAmount, loan.InterestRate, loan.LoanTerm);
        loan.TotalRepayment = loan.MonthlyPayment * loan.LoanTerm;

        // Add loan to the database
        dbContext.MortgageLoans.Add(loan);
        await dbContext.SaveChangesAsync();

        return Result<MortgageLoan>.Success(loan);
    }

    // Update an existing mortgage loan
    public async Task<Result<MortgageLoan>> UpdateMortgageLoanAsync(int id, MortgageLoan updatedLoan)
    {
        var loan = await dbContext.MortgageLoans.FindAsync(id);
        if (loan == null)
            return Result<MortgageLoan>.NotFoundError();

        // Validate input
        if (updatedLoan.LoanAmount <= 0 || updatedLoan.InterestRate <= 0 || updatedLoan.LoanTerm <= 0)
            return Result<MortgageLoan>.ValidationError("LoanAmount, InterestRate, and LoanTerm must be greater than 0.");

        // Update loan details
        loan.LoanAmount = updatedLoan.LoanAmount;
        loan.InterestRate = updatedLoan.InterestRate;
        loan.LoanTerm = updatedLoan.LoanTerm;
        loan.StartDate = updatedLoan.StartDate;
        loan.DownPayment = updatedLoan.DownPayment;

        // Recalculate monthly payment and total repayment
        loan.MonthlyPayment = CalculateMonthlyPayment(loan.LoanAmount, loan.InterestRate, loan.LoanTerm);
        loan.TotalRepayment = loan.MonthlyPayment * loan.LoanTerm;

        await dbContext.SaveChangesAsync();

        return Result<MortgageLoan>.Success(loan);
    }

    // Delete a mortgage loan
    public async Task<Result<MortgageLoan>> DeleteMortgageLoanAsync(int id)
    {
        var loan = await dbContext.MortgageLoans.FindAsync(id);
        if (loan == null)
            return Result<MortgageLoan>.NotFoundError();

        dbContext.MortgageLoans.Remove(loan);
        await dbContext.SaveChangesAsync();

        return Result<MortgageLoan>.Success(loan, "Mortgage loan deleted successfully.");
    }

    // Helper method to calculate monthly payment
    private decimal CalculateMonthlyPayment(decimal loanAmount, decimal interestRate, int loanTerm)
    {
        decimal monthlyInterestRate = interestRate / 100 / 12;
        int numberOfPayments = loanTerm * 12;

        decimal monthlyPayment = loanAmount * (monthlyInterestRate * (decimal)Math.Pow(1 + (double)monthlyInterestRate, numberOfPayments)) /
                                 ((decimal)Math.Pow(1 + (double)monthlyInterestRate, numberOfPayments) - 1);

        return Math.Round(monthlyPayment, 2);
    }
}