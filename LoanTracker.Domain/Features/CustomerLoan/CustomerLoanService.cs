using Microsoft.Extensions.Logging;
using LoanTracker.Shared.Models;
using LoanTracker.Database.AppDbContextModels;
using Microsoft.EntityFrameworkCore;

namespace LoanTracker.Domain.Features.CustomerLoan;

public class CustomerLoanService
{
    private readonly AppDbContext _context;
    private readonly ILogger<CustomerLoanService> _logger;

    public CustomerLoanService(AppDbContext context, ILogger<CustomerLoanService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<CustomerLoanResponse>> CreateLoanAsync(CustomerLoanRequest request)
    {
        try
        {
            _logger.LogInformation("Attempting to create loan for Customer ID: {CustomerId}, Loan Type ID: {LoanTypeId}", request.CustomerId, request.LoanTypeId);

            var loanType = await _context.TblLoanTypes.FindAsync(request.LoanTypeId);
            if (loanType == null)
            {
                _logger.LogWarning("Loan creation failed: Loan type with ID {LoanTypeId} not found.", request.LoanTypeId);
                return Result<CustomerLoanResponse>.ValidationError("Loan type not found.");
            }

            var loan = new TblCustomerLoan
            {
                CustomerId = request.CustomerId,
                LoanTypeId = request.LoanTypeId,
                PrincipalAmount = request.PrincipalAmount,
                InterestRate = request.InterestRate,
                LoanTerm = request.LoanTerm,
                LoanStartDate = DateOnly.FromDateTime(request.LoanStartDate),
                RepaymentFrequency = request.RepaymentFrequency ?? "Monthly",
                Status = "Active",
                CreatedDate = DateTime.UtcNow,
                TotalAmount = CalculateTotalAmount(request)
            };

            _context.TblCustomerLoans.Add(loan);
            await _context.SaveChangesAsync();

            await GeneratePaymentSchedulesAsync(loan);

            _logger.LogInformation("Loan created successfully with ID: {LoanId}", loan.LoanId);
            
            var createdLoan = await GetLoanByIdWithIncludesAsync(loan.LoanId);
            return Result<CustomerLoanResponse>.Success(MapToResponse(createdLoan!), "Loan created successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating loan for Customer ID: {CustomerId}", request.CustomerId);
            return Result<CustomerLoanResponse>.Failure("An error occurred while creating the loan.");
        }
    }

    public async Task<Result<CustomerLoanResponse>> GetLoanByIdAsync(int loanId)
    {
        try
        {
            _logger.LogInformation("Fetching loan by ID: {LoanId}", loanId);
            var loan = await GetLoanByIdWithIncludesAsync(loanId);

            if (loan == null)
            {
                _logger.LogWarning("Loan with ID {LoanId} not found.", loanId);
                return Result<CustomerLoanResponse>.NotFoundError("Loan not found.");
            }

            return Result<CustomerLoanResponse>.Success(MapToResponse(loan), "Loan retrieved successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching loan by ID: {LoanId}", loanId);
            return Result<CustomerLoanResponse>.Failure("An error occurred while retrieving the loan.");
        }
    }

    public async Task<Result<List<CustomerLoanResponse>>> GetAllLoansAsync()
    {
        try
        {
            _logger.LogInformation("Fetching all loans.");
            var loans = await _context.TblCustomerLoans
                .Include(l => l.Customer)
                .Include(l => l.LoanType)
                .ToListAsync();

            var response = loans.Select(MapToResponse).ToList();
            _logger.LogInformation("Retrieved {Count} loans.", response.Count);
            return Result<List<CustomerLoanResponse>>.Success(response, "Loans retrieved successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching all loans.");
            return Result<List<CustomerLoanResponse>>.Failure("An error occurred while retrieving all loans.");
        }
    }

    public async Task<Result<CustomerLoanResponse>> UpdateLoanAsync(int loanId, CustomerLoanRequest request)
    {
        try
        {
            _logger.LogInformation("Attempting to update loan with ID: {LoanId}", loanId);

            var existingLoan = await _context.TblCustomerLoans
                .Include(l => l.TblPaymentSchedules)
                .FirstOrDefaultAsync(l => l.LoanId == loanId);

            if (existingLoan == null)
            {
                _logger.LogWarning("Update failed: Loan with ID {LoanId} not found.", loanId);
                return Result<CustomerLoanResponse>.NotFoundError("Loan not found.");
            }

            existingLoan.CustomerId = request.CustomerId;
            existingLoan.LoanTypeId = request.LoanTypeId;
            existingLoan.PrincipalAmount = request.PrincipalAmount;
            existingLoan.InterestRate = request.InterestRate;
            existingLoan.LoanTerm = request.LoanTerm;
            existingLoan.LoanStartDate = DateOnly.FromDateTime(request.LoanStartDate);
            existingLoan.RepaymentFrequency = request.RepaymentFrequency;
            existingLoan.Status = request.Status;
            existingLoan.TotalAmount = CalculateTotalAmount(request);

            await RegeneratePaymentSchedulesAsync(existingLoan);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Loan with ID {LoanId} updated successfully.", loanId);
            
            var updatedLoan = await GetLoanByIdWithIncludesAsync(loanId);
            return Result<CustomerLoanResponse>.Success(MapToResponse(updatedLoan!), "Loan updated successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating loan with ID: {LoanId}", loanId);
            return Result<CustomerLoanResponse>.Failure("An error occurred while updating the loan.");
        }
    }

    public async Task<Result<bool>> DeleteLoanAsync(int loanId)
    {
        try
        {
            _logger.LogInformation("Attempting to delete loan with ID: {LoanId}", loanId);
            var loan = await _context.TblCustomerLoans
                .Include(l => l.TblPaymentSchedules)
                .FirstOrDefaultAsync(l => l.LoanId == loanId);

            if (loan == null)
            {
                _logger.LogWarning("Delete failed: Loan with ID {LoanId} not found.", loanId);
                return Result<bool>.NotFoundError("Loan not found.");
            }

            _context.TblPaymentSchedules.RemoveRange(loan.TblPaymentSchedules);
            _context.TblCustomerLoans.Remove(loan);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Loan with ID {LoanId} and its schedules deleted successfully.", loanId);
            return Result<bool>.Success(true, "Loan deleted successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting loan with ID: {LoanId}", loanId);
            return Result<bool>.Failure("An error occurred while deleting the loan.");
        }
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

        _context.TblPaymentSchedules.AddRange(paymentSchedules);
        await _context.SaveChangesAsync();
    }

    private async Task RegeneratePaymentSchedulesAsync(TblCustomerLoan loan)
    {
        var existingSchedules = await _context.TblPaymentSchedules
            .Where(s => s.LoanId == loan.LoanId)
            .ToListAsync();

        _context.TblPaymentSchedules.RemoveRange(existingSchedules);
        await _context.SaveChangesAsync();

        await GeneratePaymentSchedulesAsync(loan);
    }

    private decimal CalculateTotalAmount(CustomerLoanRequest request)
    {
        // Simplistic total calculation (Principal + (Principal * Rate * Term / 12))
        return request.PrincipalAmount + (request.PrincipalAmount * (request.InterestRate / 100) * (request.LoanTerm / 12.0m));
    }

    private async Task<TblCustomerLoan?> GetLoanByIdWithIncludesAsync(int loanId)
    {
        return await _context.TblCustomerLoans
            .Include(l => l.Customer)
            .Include(l => l.LoanType)
            .FirstOrDefaultAsync(l => l.LoanId == loanId);
    }

    private static CustomerLoanResponse MapToResponse(TblCustomerLoan loan)
    {
        return new CustomerLoanResponse
        {
            LoanId = loan.LoanId,
            CustomerId = loan.CustomerId,
            CustomerName = loan.Customer?.CustomerName ?? "Unknown",
            LoanTypeId = loan.LoanTypeId,
            LoanTypeName = loan.LoanType?.LoanTypeName ?? "Unknown",
            TotalAmount = loan.TotalAmount,
            PrincipalAmount = loan.PrincipalAmount,
            InterestRate = loan.InterestRate,
            LoanTerm = loan.LoanTerm,
            LoanStartDate = loan.LoanStartDate.ToDateTime(TimeOnly.MinValue),
            RepaymentFrequency = loan.RepaymentFrequency,
            Status = loan.Status
        };
    }
}
