using Microsoft.EntityFrameworkCore;
using Moq;
using LoanTracker.Database.AppDbContextModels;
using LoanTracker.Domain;
using System.Threading;
using Xunit;
using System.Linq.Expressions;
using LoanTracker.Domain.Features;

namespace LoanTracker.UnitTests;

public class PaymentServiceTests
{
    private readonly Mock<AppDbContext> _mockDbContext;
    private readonly Mock<LateFeeRuleService> _mockLateFeeRuleService;
    private readonly PaymentService _paymentService;

    public PaymentServiceTests()
    {
        _mockDbContext = new Mock<AppDbContext>();
        _mockLateFeeRuleService = new Mock<LateFeeRuleService>(_mockDbContext.Object);
        _paymentService = new PaymentService(_mockDbContext.Object, _mockLateFeeRuleService.Object);
    }

    [Fact]
    public async Task RecordPaymentAsync_LoanNotFound_ReturnsNotFoundError()
    {
        // Arrange
        int loanId = 1;
        DateOnly paymentDate = DateOnly.FromDateTime(DateTime.Now);
        decimal amountPaid = 100;

        _mockDbContext.Setup(db => db.MortgageLoans.FindAsync(loanId))
                      .ReturnsAsync((MortgageLoan)null);

        // Act
        var result = await _paymentService.RecordPaymentAsync(loanId, paymentDate, amountPaid);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Loan not found.", result.Message);
    }

    //[Fact]
    //public async Task RecordPaymentAsync_ExceedsTotalRepayment_ReturnsValidationError()
    //{
    //    // Arrange
    //    int loanId = 1;
    //    DateOnly paymentDate = DateOnly.FromDateTime(DateTime.Now);
    //    decimal amountPaid = 100;
    //    var loan = new MortgageLoan { LoanId = loanId, TotalRepayment = 500 };

    //    _mockDbContext.Setup(db => db.MortgageLoans.FindAsync(loanId))
    //                  .ReturnsAsync(loan);

    //    // Mock the Payments DbSet to return a queryable list
    //    var payments = new List<Payment>
    //{
    //    new Payment { PaymentId = 1, LoanId = loanId, AmountPaid = 450 }
    //};

    //    var mockPaymentDbSet = new Mock<DbSet<Payment>>();
    //    mockPaymentDbSet.As<IQueryable<Payment>>()
    //                    .Setup(m => m.Provider)
    //                    .Returns(payments.AsQueryable().Provider);

    //    mockPaymentDbSet.As<IQueryable<Payment>>()
    //                    .Setup(m => m.Expression)
    //                    .Returns(payments.AsQueryable().Expression);

    //    mockPaymentDbSet.As<IQueryable<Payment>>()
    //                    .Setup(m => m.ElementType)
    //                    .Returns(payments.AsQueryable().ElementType);

    //    mockPaymentDbSet.As<IQueryable<Payment>>()
    //                    .Setup(m => m.GetEnumerator())
    //                    .Returns(payments.GetEnumerator());

    //    _mockDbContext.Setup(db => db.Payments).Returns(mockPaymentDbSet.Object);

    //    // Mock the LateFeeRules DbSet to support async operations
    //    var lateFeeRules = new List<LateFeeRule>
    //{
    //    new LateFeeRule { RuleId = 1, MinDaysOverdue = 1, MaxDaysOverdue = 10, LateFeeAmount = 25.00m },
    //    new LateFeeRule { RuleId = 2, MinDaysOverdue = 11, MaxDaysOverdue = 20, LateFeeAmount = 50.00m }
    //};

    //    var mockLateFeeRuleDbSet = new Mock<DbSet<LateFeeRule>>();
    //    mockLateFeeRuleDbSet.As<IAsyncEnumerable<LateFeeRule>>()
    //                        .Setup(m => m.GetAsyncEnumerator(default))
    //                        .Returns(new TestAsyncEnumerator<LateFeeRule>(lateFeeRules.GetEnumerator()));

    //    mockLateFeeRuleDbSet.As<IQueryable<LateFeeRule>>()
    //                        .Setup(m => m.Provider)
    //                        .Returns(new TestAsyncQueryProvider<LateFeeRule>(lateFeeRules.AsQueryable().Provider));

    //    mockLateFeeRuleDbSet.As<IQueryable<LateFeeRule>>()
    //                        .Setup(m => m.Expression)
    //                        .Returns(lateFeeRules.AsQueryable().Expression);

    //    mockLateFeeRuleDbSet.As<IQueryable<LateFeeRule>>()
    //                        .Setup(m => m.ElementType)
    //                        .Returns(lateFeeRules.AsQueryable().ElementType);

    //    mockLateFeeRuleDbSet.As<IQueryable<LateFeeRule>>()
    //                        .Setup(m => m.GetEnumerator())
    //                        .Returns(lateFeeRules.GetEnumerator());

    //    _mockDbContext.Setup(db => db.LateFeeRules).Returns(mockLateFeeRuleDbSet.Object);

    //    // Act
    //    var result = await _paymentService.RecordPaymentAsync(loanId, paymentDate, amountPaid);

    //    // Assert
    //    Assert.False(result.IsSuccess);
    //    Assert.Equal("Payment exceeds the total repayment amount.", result.Message);
    //}

    //[Fact]
    //public async Task RecordPaymentAsync_Success_ReturnsPayment()
    //{
    //    // Arrange
    //    int loanId = 1;
    //    DateOnly paymentDate = DateOnly.FromDateTime(DateTime.Now);
    //    decimal amountPaid = 100;
    //    var loan = new MortgageLoan { LoanId = loanId, TotalRepayment = 500 };

    //    _mockDbContext.Setup(db => db.MortgageLoans.FindAsync(loanId))
    //                  .ReturnsAsync(loan);

    //    // Mock the Payments DbSet to return a queryable list
    //    var payments = new List<Payment>
    //{
    //    new Payment { PaymentId = 1, LoanId = loanId, AmountPaid = 300 }
    //};

    //    var mockPaymentDbSet = new Mock<DbSet<Payment>>();
    //    mockPaymentDbSet.As<IQueryable<Payment>>()
    //                    .Setup(m => m.Provider)
    //                    .Returns(payments.AsQueryable().Provider);

    //    mockPaymentDbSet.As<IQueryable<Payment>>()
    //                    .Setup(m => m.Expression)
    //                    .Returns(payments.AsQueryable().Expression);

    //    mockPaymentDbSet.As<IQueryable<Payment>>()
    //                    .Setup(m => m.ElementType)
    //                    .Returns(payments.AsQueryable().ElementType);

    //    mockPaymentDbSet.As<IQueryable<Payment>>()
    //                    .Setup(m => m.GetEnumerator())
    //                    .Returns(payments.GetEnumerator());

    //    _mockDbContext.Setup(db => db.Payments).Returns(mockPaymentDbSet.Object);

    //    var lateFee = 10;
    //    _mockLateFeeRuleService.Setup(service => service.GetAllLateFeeRuleAsync())
    //                           .ReturnsAsync(Result<List<LateFeeRule>>.Success(new List<LateFeeRule>
    //                           {
    //                           new LateFeeRule { MinDaysOverdue = 1, MaxDaysOverdue = null, LateFeeAmount = lateFee }
    //                           }));

    //    _mockDbContext.Setup(db => db.Payments.Add(It.IsAny<Payment>()));
    //    _mockDbContext.Setup(db => db.SaveChangesAsync(It.IsAny<CancellationToken>()))
    //                  .ReturnsAsync(1);

    //    // Act
    //    var result = await _paymentService.RecordPaymentAsync(loanId, paymentDate, amountPaid);

    //    // Assert
    //    Assert.True(result.IsSuccess);
    //    Assert.NotNull(result.Data);
    //    Assert.Equal(loanId, result.Data.LoanId);
    //    Assert.Equal(paymentDate, result.Data.PaymentDate);
    //    Assert.Equal(amountPaid, result.Data.AmountPaid);
    //    Assert.Equal(lateFee, result.Data.LateFee);
    //}

    [Fact]
    public async Task GeneratePaymentScheduleAsync_ReturnsPaymentSchedule()
    {
        // Arrange
        var loan = new MortgageLoan
        {
            LoanId = 1,
            CustomerId = 1,
            LoanAmount = 100000,
            InterestRate = 5.0m,
            LoanTerm = 30,
            StartDate = new DateOnly(2023, 10, 1),
            DownPayment = 20000,
            MonthlyPayment = 536.82m
        };

        var mockDbSet = new Mock<DbSet<MortgageLoan>>();
        mockDbSet.Setup(m => m.FindAsync(1)).ReturnsAsync(loan);

        _mockDbContext.Setup(db => db.MortgageLoans).Returns(mockDbSet.Object);

        // Act
        var result = await _paymentService.GeneratePaymentScheduleAsync(1, "monthly");

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(360, result.Data.Count); // 30 years * 12 months
    }
}
