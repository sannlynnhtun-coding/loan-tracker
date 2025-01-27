using Microsoft.EntityFrameworkCore;
using Moq;
using LoanTracker.Database.AppDbContextModels;
using LoanTracker.Domain;
using LoanTracker.Domain.Features;

namespace LoanTracker.UnitTests;

public class MortgageLoanServiceTests
{
    private readonly Mock<AppDbContext> _mockDbContext;
    private readonly MortgageLoanService _mortgageLoanService;

    public MortgageLoanServiceTests()
    {
        _mockDbContext = new Mock<AppDbContext>();
        _mortgageLoanService = new MortgageLoanService(_mockDbContext.Object);
    }

    [Fact]
    public async Task CreateMortgageLoanAsync_ReturnsCreatedLoan()
    {
        // Arrange
        var loan = new MortgageLoan
        {
            CustomerId = 1,
            LoanAmount = 100000,
            InterestRate = 5.0m,
            LoanTerm = 30,
            StartDate = new DateOnly(2023, 10, 1),
            DownPayment = 20000
        };

        var mockDbSet = new Mock<DbSet<MortgageLoan>>();
        _mockDbContext.Setup(db => db.MortgageLoans).Returns(mockDbSet.Object);

        // Act
        var result = await _mortgageLoanService.CreateMortgageLoanAsync(loan);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(100000, result.Data.LoanAmount);
    }

    [Fact]
    public async Task UpdateMortgageLoanAsync_ReturnsUpdatedLoan()
    {
        // Arrange
        var existingLoan = new MortgageLoan
        {
            LoanId = 1,
            CustomerId = 1,
            LoanAmount = 100000,
            InterestRate = 5.0m,
            LoanTerm = 30,
            StartDate = new DateOnly(2023, 10, 1),
            DownPayment = 20000
        };

        var updatedLoan = new MortgageLoan
        {
            LoanId = 1,
            CustomerId = 1,
            LoanAmount = 120000,
            InterestRate = 4.5m,
            LoanTerm = 25,
            StartDate = new DateOnly(2023, 10, 1),
            DownPayment = 25000
        };

        var mockDbSet = new Mock<DbSet<MortgageLoan>>();
        mockDbSet.Setup(m => m.FindAsync(1)).ReturnsAsync(existingLoan);

        _mockDbContext.Setup(db => db.MortgageLoans).Returns(mockDbSet.Object);

        // Act
        var result = await _mortgageLoanService.UpdateMortgageLoanAsync(1, updatedLoan);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(120000, result.Data.LoanAmount);
    }
}
