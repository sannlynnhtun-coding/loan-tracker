using LoanTracker.Domain.Features.LoanType;

public class GetLoanTypeService
{
    private readonly AppDbContext _context;

    public GetLoanTypeService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<LoanTypeResponse>> HandleAsync(GetLoanTypeRequest request)
    {
        var loanType = await _context.TblLoanTypes
            .Where(l => l.LoanTypeId == request.LoanTypeId)
            .Select(l => new LoanTypeResponse
            {
                LoanTypeId = l.LoanTypeId,
                LoanTypeName = l.LoanTypeName,
                Description = l.Description,
                CreatedDate = l.CreatedDate
            })
            .FirstOrDefaultAsync();

        return loanType == null
            ? Result<LoanTypeResponse>.NotFoundError("Loan Type not found.")
            : Result<LoanTypeResponse>.Success(loanType, "Loan Type retrieved successfully.");
    }
}