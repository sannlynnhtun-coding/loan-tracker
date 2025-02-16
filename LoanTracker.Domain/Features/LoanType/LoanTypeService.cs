namespace LoanTracker.Application.Services;

public class LoanTypeService
{
    private readonly AppDbContext _context;

    public LoanTypeService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<TblLoanType>> AddLoanTypeAsync(TblLoanType loanType, TblLoanTypeBurmese burmeseLoanType)
    {
        if (await _context.TblLoanTypes.AnyAsync(lt => lt.LoanTypeName == loanType.LoanTypeName))
            return Result<TblLoanType>.ValidationError("English loan type name already exists.");

        if (await _context.TblLoanTypeBurmeses.AnyAsync(ltb => ltb.LoanTypeName == burmeseLoanType.LoanTypeName))
            return Result<TblLoanType>.ValidationError("Burmese loan type name already exists.");

        _context.TblLoanTypes.Add(loanType);
        _context.TblLoanTypeBurmeses.Add(burmeseLoanType);
        await _context.SaveChangesAsync();
        return Result<TblLoanType>.Success(loanType, "Loan type added successfully.");
    }

    public async Task<Result<TblLoanType>> GetLoanTypeByIdAsync(int loanTypeId)
    {
        var loanType = await _context.TblLoanTypes.FindAsync(loanTypeId);
        if (loanType == null)
            return Result<TblLoanType>.NotFoundError("Loan type not found.");

        return Result<TblLoanType>.Success(loanType, "Loan type retrieved successfully.");
    }

    public async Task<Result<List<TblLoanType>>> GetAllLoanTypesAsync()
    {
        var loanTypes = await _context.TblLoanTypes.ToListAsync();
        return Result<List<TblLoanType>>.Success(loanTypes, "Loan types retrieved successfully.");
    }

    public async Task<Result<TblLoanType>> UpdateLoanTypeAsync(TblLoanType loanType)
    {
        _context.TblLoanTypes.Update(loanType);
        await _context.SaveChangesAsync();
        return Result<TblLoanType>.Success(loanType, "Loan type updated successfully.");
    }

    public async Task<Result<bool>> DeleteLoanTypeAsync(int loanTypeId)
    {
        var loanType = await _context.TblLoanTypes.FindAsync(loanTypeId);
        if (loanType == null)
            return Result<bool>.NotFoundError("Loan type not found.");

        _context.TblLoanTypes.Remove(loanType);
        await _context.SaveChangesAsync();
        return Result<bool>.Success(true, "Loan type deleted successfully.");
    }
}