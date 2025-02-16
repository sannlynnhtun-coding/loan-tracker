using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoanTracker.Domain.Features.LoanType;

namespace LoanTracker.Domain.Features.LoanType
{
}

public class CreateLoanTypeService
{
    private readonly AppDbContext _context;

    public CreateLoanTypeService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<LoanTypeResponse>> HandleAsync(CreateLoanTypeRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.LoanTypeName))
        {
            return Result<LoanTypeResponse>.ValidationError("Loan Type Name is required.");
        }

        bool exists = await _context.TblLoanTypes.AnyAsync(l => l.LoanTypeName == request.LoanTypeName);
        if (exists)
        {
            return Result<LoanTypeResponse>.ValidationError("Loan Type already exists.");
        }

        var loanType = new TblLoanType
        {
            LoanTypeName = request.LoanTypeName,
            Description = request.Description,
            CreatedDate = DateTime.UtcNow
        };

        _context.TblLoanTypes.Add(loanType);
        await _context.SaveChangesAsync();

        var response = new LoanTypeResponse
        {
            LoanTypeId = loanType.LoanTypeId,
            LoanTypeName = loanType.LoanTypeName,
            Description = loanType.Description,
            CreatedDate = loanType.CreatedDate
        };

        return Result<LoanTypeResponse>.Success(response, "Loan Type created successfully.");
    }
}