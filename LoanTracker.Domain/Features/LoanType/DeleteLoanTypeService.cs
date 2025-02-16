public class DeleteLoanTypeService
{
    private readonly AppDbContext _context;

    public DeleteLoanTypeService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<bool>> HandleAsync(DeleteLoanTypeRequest request)
    {
        var loanType = await _context.TblLoanTypes.FindAsync(request.LoanTypeId);
        if (loanType == null)
        {
            return Result<bool>.NotFoundError("Loan Type not found.");
        }

        _context.TblLoanTypes.Remove(loanType);
        await _context.SaveChangesAsync();
        return Result<bool>.Success(true, "Loan Type deleted successfully.");
    }
}