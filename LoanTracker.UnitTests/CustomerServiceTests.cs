using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using LoanTracker.Database.AppDbContextModels;
using LoanTracker.Domain;
using LoanTracker.Domain.Features;

namespace LoanTracker.UnitTests;

public class CustomerServiceTests
{
    private readonly Mock<AppDbContext> _mockDbContext;
    private readonly CustomerService _customerService;

    public CustomerServiceTests()
    {
        _mockDbContext = new Mock<AppDbContext>();
        _customerService = new CustomerService(_mockDbContext.Object);
    }

    [Fact]
    public async Task GetAllCustomersAsync_ReturnsListOfCustomers()
    {
        // Arrange
        var customers = new List<Customer>
        {
            new Customer { CustomerId = 1, BorrowerName = "John Doe", Nrc = "123456789" },
            new Customer { CustomerId = 2, BorrowerName = "Jane Doe", Nrc = "987654321" }
        };

        var mockDbSet = new Mock<DbSet<Customer>>();
        mockDbSet.As<IAsyncEnumerable<Customer>>()
            .Setup(m => m.GetAsyncEnumerator(default))
            .Returns(new TestAsyncEnumerator<Customer>(customers.GetEnumerator()));

        mockDbSet.As<IQueryable<Customer>>()
            .Setup(m => m.Provider)
            .Returns(new TestAsyncQueryProvider<Customer>(customers.AsQueryable().Provider));

        mockDbSet.As<IQueryable<Customer>>()
            .Setup(m => m.Expression)
            .Returns(customers.AsQueryable().Expression);

        mockDbSet.As<IQueryable<Customer>>()
            .Setup(m => m.ElementType)
            .Returns(customers.AsQueryable().ElementType);

        mockDbSet.As<IQueryable<Customer>>()
            .Setup(m => m.GetEnumerator())
            .Returns(customers.GetEnumerator());

        _mockDbContext.Setup(db => db.Customers).Returns(mockDbSet.Object);

        // Act
        var result = await _customerService.GetAllCustomersAsync();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Data.Count);
    }

    [Fact]
    public async Task GetCustomerByIdAsync_ReturnsCustomer()
    {
        // Arrange
        var customer = new Customer { CustomerId = 1, BorrowerName = "John Doe", Nrc = "123456789" };

        var mockDbSet = new Mock<DbSet<Customer>>();
        mockDbSet.Setup(m => m.FindAsync(1)).ReturnsAsync(customer);

        _mockDbContext.Setup(db => db.Customers).Returns(mockDbSet.Object);

        // Act
        var result = await _customerService.GetCustomerByIdAsync(1);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("John Doe", result.Data.BorrowerName);
    }

    [Fact]
    public async Task CreateCustomerAsync_ReturnsCreatedCustomer()
    {
        // Arrange
        var customer = new Customer { BorrowerName = "John Doe", Nrc = "123456789" };

        var mockDbSet = new Mock<DbSet<Customer>>();
        _mockDbContext.Setup(db => db.Customers).Returns(mockDbSet.Object);

        // Act
        var result = await _customerService.CreateCustomerAsync(customer);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("John Doe", result.Data.BorrowerName);
    }
}
