using Microsoft.Extensions.Logging;
using LoanTracker.Shared.Models;
using LoanTracker.Database.AppDbContextModels;
using Microsoft.EntityFrameworkCore;

namespace LoanTracker.Application.Services;

public class LoanTypeService
{
    private readonly AppDbContext _context;
    private readonly ILogger<LoanTypeService> _logger;

    public LoanTypeService(AppDbContext context, ILogger<LoanTypeService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<LoanTypeResponse>> AddLoanTypeAsync(LoanTypeRequest request)
    {
        try
        {
            _logger.LogInformation("Attempting to add loan type: {LoanTypeName}", request.LoanTypeName);

            if (await _context.TblLoanTypes.AnyAsync(lt => lt.LoanTypeName == request.LoanTypeName))
            {
                _logger.LogWarning("Validation failed: English loan type name {Name} already exists.", request.LoanTypeName);
                return Result<LoanTypeResponse>.ValidationError("English loan type name already exists.");
            }

            if (await _context.TblLoanTypeBurmeses.AnyAsync(ltb => ltb.LoanTypeName == request.BurmeseLoanTypeName))
            {
                _logger.LogWarning("Validation failed: Burmese loan type name {Name} already exists.", request.BurmeseLoanTypeName);
                return Result<LoanTypeResponse>.ValidationError("Burmese loan type name already exists.");
            }

            var loanType = new TblLoanType
            {
                LoanTypeName = request.LoanTypeName,
                Description = request.Description,
                CreatedDate = DateTime.UtcNow
            };

            _context.TblLoanTypes.Add(loanType);
            await _context.SaveChangesAsync(); // Need ID for Burmese mapping

            var burmeseLoanType = new TblLoanTypeBurmese
            {
                LoanTypeName = request.BurmeseLoanTypeName,
                Description = request.BurmeseDescription,
                EnglishLoanTypeId = loanType.LoanTypeId,
                CreatedDate = DateTime.UtcNow
            };

            _context.TblLoanTypeBurmeses.Add(burmeseLoanType);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Loan type added successfully with ID: {LoanTypeId}", loanType.LoanTypeId);
            
            // Re-fetch with Burmese data for consistent mapping
            var createdLoan = await _context.TblLoanTypes
                .Include(lt => lt.TblLoanTypeBurmeses)
                .FirstOrDefaultAsync(lt => lt.LoanTypeId == loanType.LoanTypeId);

            return Result<LoanTypeResponse>.Success(MapToResponse(createdLoan!), "Loan type added successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while adding loan type: {LoanTypeName}", request.LoanTypeName);
            return Result<LoanTypeResponse>.Failure("An error occurred while adding the loan type.");
        }
    }

    public async Task<Result<LoanTypeResponse>> GetLoanTypeByIdAsync(int loanTypeId)
    {
        try
        {
            _logger.LogInformation("Fetching loan type by ID: {LoanTypeId}", loanTypeId);
            var loanType = await _context.TblLoanTypes
                .Include(lt => lt.TblLoanTypeBurmeses)
                .FirstOrDefaultAsync(lt => lt.LoanTypeId == loanTypeId);

            if (loanType == null)
            {
                _logger.LogWarning("Loan type with ID {LoanTypeId} not found.", loanTypeId);
                return Result<LoanTypeResponse>.NotFoundError("Loan type not found.");
            }

            return Result<LoanTypeResponse>.Success(MapToResponse(loanType), "Loan type retrieved successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching loan type by ID: {LoanTypeId}", loanTypeId);
            return Result<LoanTypeResponse>.Failure("An error occurred while retrieving the loan type.");
        }
    }

    public async Task<Result<List<LoanTypeResponse>>> GetAllLoanTypesAsync()
    {
        try
        {
            _logger.LogInformation("Fetching all loan types.");
            var loanTypes = await _context.TblLoanTypes
                .Include(lt => lt.TblLoanTypeBurmeses)
                .ToListAsync();

            var response = loanTypes.Select(lt => MapToResponse(lt)).ToList();

            _logger.LogInformation("Retrieved {Count} loan types.", response.Count);
            return Result<List<LoanTypeResponse>>.Success(response, "Loan types retrieved successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching all loan types.");
            return Result<List<LoanTypeResponse>>.Failure("An error occurred while retrieving all loan types.");
        }
    }

    public async Task<Result<LoanTypeResponse>> UpdateLoanTypeAsync(LoanTypeRequest request)
    {
        try
        {
            _logger.LogInformation("Attempting to update loan type with ID: {LoanTypeId}", request.LoanTypeId);
            
            var loanType = await _context.TblLoanTypes
                .Include(lt => lt.TblLoanTypeBurmeses)
                .FirstOrDefaultAsync(lt => lt.LoanTypeId == request.LoanTypeId);

            if (loanType == null)
            {
                return Result<LoanTypeResponse>.NotFoundError("Loan type not found.");
            }

            loanType.LoanTypeName = request.LoanTypeName;
            loanType.Description = request.Description;

            var burmese = loanType.TblLoanTypeBurmeses.FirstOrDefault();
            if (burmese != null)
            {
                burmese.LoanTypeName = request.BurmeseLoanTypeName;
                burmese.Description = request.BurmeseDescription;
            }

            _context.TblLoanTypes.Update(loanType);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Loan type with ID {LoanTypeId} updated successfully.", loanType.LoanTypeId);
            return Result<LoanTypeResponse>.Success(MapToResponse(loanType), "Loan type updated successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating loan type with ID: {LoanTypeId}", request.LoanTypeId);
            return Result<LoanTypeResponse>.Failure("An error occurred while updating the loan type.");
        }
    }

    public async Task<Result<bool>> DeleteLoanTypeAsync(int loanTypeId)
    {
        try
        {
            _logger.LogInformation("Attempting to delete loan type with ID: {LoanTypeId}", loanTypeId);
            var loanType = await _context.TblLoanTypes.FindAsync(loanTypeId);
            if (loanType == null)
            {
                _logger.LogWarning("Delete failed: Loan type with ID {LoanTypeId} not found.", loanTypeId);
                return Result<bool>.NotFoundError("Loan type not found.");
            }

            _context.TblLoanTypes.Remove(loanType);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Loan type with ID {LoanTypeId} deleted successfully.", loanTypeId);
            return Result<bool>.Success(true, "Loan type deleted successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting loan type with ID: {LoanTypeId}", loanTypeId);
            return Result<bool>.Failure("An error occurred while deleting the loan type.");
        }
    }

    private static LoanTypeResponse MapToResponse(TblLoanType loanType)
    {
        var burmese = loanType.TblLoanTypeBurmeses.FirstOrDefault();
        return new LoanTypeResponse
        {
            LoanTypeId = loanType.LoanTypeId,
            LoanTypeName = loanType.LoanTypeName,
            Description = loanType.Description,
            BurmeseLoanTypeName = burmese?.LoanTypeName ?? string.Empty,
            BurmeseDescription = burmese?.Description,
            CreatedDate = loanType.CreatedDate
        };
    }
}
