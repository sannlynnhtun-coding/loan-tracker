using LoanTracker.Domain.Features.LoanType;

public class UpdateLoanTypeService
{
    private readonly AppDbContext _context;

    public UpdateLoanTypeService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<bool>> HandleAsync(int id, UpdateLoanTypeRequest request)
    {
        var loanType = await _context.TblLoanTypes.FindAsync(id);
        if (loanType == null)
        {
            return Result<bool>.NotFoundError("Loan Type not found.");
        }

        loanType.LoanTypeName = request.LoanTypeName;
        loanType.Description = request.Description;

        await _context.SaveChangesAsync();
        return Result<bool>.Success(true, "Loan Type updated successfully.");
    }
}