using LoanTracker.Database.AppDbContextModels;
using LoanTracker.Domain;
using LoanTracker.Domain.Features;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Add DbContext (Database First)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<LateFeeRuleService>();
builder.Services.AddScoped<MortgageLoanService>();
builder.Services.AddScoped<PaymentService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

#region CRUD Endpoints for Customer

app.MapGet("/customers", async (CustomerService customerService) =>
{
    var result = await customerService.GetAllCustomersAsync();
    return result.Execute();
});

app.MapGet("/customers/{id}", async (int id, CustomerService customerService) =>
{
    var result = await customerService.GetCustomerByIdAsync(id);
    return result.Execute();
});

app.MapPost("/customers", async (Customer customer, CustomerService customerService) =>
{
    var result = await customerService.CreateCustomerAsync(customer);
    return result.Execute();
});

app.MapPut("/customers/{id}", async (int id, Customer updatedCustomer, CustomerService customerService) =>
{
    var result = await customerService.UpdateCustomerAsync(id, updatedCustomer);
    return result.Execute();
});

app.MapDelete("/customers/{id}", async (int id, CustomerService customerService) =>
{
    var result = await customerService.DeleteCustomerAsync(id);
    return result.Execute();
});

#endregion

#region CRUD Endpoints for LateFeeRules

app.MapGet("/latefeerules", async (LateFeeRuleService lateFeeRuleService) =>
{
    var result = await lateFeeRuleService.GetAllLateFeeRuleAsync();
    return result.Execute();
});

app.MapGet("/latefeerules/{id}", async (int id, LateFeeRuleService lateFeeRuleService) =>
{
    var result = await lateFeeRuleService.GetLateFeeRuleByIdAsync(id);
    return result.Execute();
});

app.MapPost("/latefeerules", async (LateFeeRule rule, LateFeeRuleService lateFeeRuleService) =>
{
    var result = await lateFeeRuleService.CreateLateFeeRuleAsync(rule);
    return result.Execute();
});

app.MapPut("/latefeerules/{id}", async (int id, LateFeeRule updatedRule, LateFeeRuleService lateFeeRuleService) =>
{
    var result = await lateFeeRuleService.UpdateLateFeeRuleAsync(id, updatedRule);
    return result.Execute();
});

app.MapDelete("/latefeerules/{id}", async (int id, LateFeeRuleService lateFeeRuleService) =>
{
    var result = await lateFeeRuleService.DeleteLateFeeRuleAsync(id);
    return result.Execute();
});

#endregion

#region CRUD Endpoints for MortgageLoan

app.MapGet("/mortgageloans", async (MortgageLoanService mortgageLoanService) =>
{
    var result = await mortgageLoanService.GetAllMortgageLoansAsync();
    return result.Execute();
});

app.MapGet("/mortgageloans/{id}", async (int id, MortgageLoanService mortgageLoanService) =>
{
    var result = await mortgageLoanService.GetMortgageLoanByIdAsync(id);
    return result.Execute();
});

app.MapPost("/mortgageloans", async (MortgageLoan loan, MortgageLoanService mortgageLoanService) =>
{
    var result = await mortgageLoanService.CreateMortgageLoanAsync(loan);
    return result.Execute();
});

app.MapPut("/mortgageloans/{id}", async (int id, MortgageLoan updatedLoan, MortgageLoanService mortgageLoanService) =>
{
    var result = await mortgageLoanService.UpdateMortgageLoanAsync(id, updatedLoan);
    return result.Execute();
});

app.MapDelete("/mortgageloans/{id}", async (int id, MortgageLoanService mortgageLoanService) =>
{
    var result = await mortgageLoanService.DeleteMortgageLoanAsync(id);
    return result.Execute();
});

#endregion

#region Payment Endpoints

app.MapGet("/payments/{loanId}", async (int loanId, PaymentService paymentService) =>
{
    var result = await paymentService.GetPaymentsByLoanIdAsync(loanId);
    return result.Execute();
});

app.MapPost("/payments", async (int loanId, DateOnly paymentDate, decimal amountPaid, PaymentService paymentService) =>
{
    var result = await paymentService.RecordPaymentAsync(loanId, paymentDate, amountPaid);
    return result.Execute();
});

app.MapGet("/payments/schedule/{loanId}", async (int loanId, string scheduleType, PaymentService paymentService) =>
{
    var result = await paymentService.GeneratePaymentScheduleAsync(loanId, scheduleType);
    return result.Execute();
});

#endregion

app.Run();
