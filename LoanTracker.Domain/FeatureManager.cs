namespace LoanTracker.Domain;

public static class FeatureManager
{
    public static void AddDomain(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<CustomerService>();
        builder.Services.AddScoped<CustomerLoanService>();
        builder.Services.AddScoped<CustomerPaymentService>();
        builder.Services.AddScoped<LoanTypeService>();
    }
}