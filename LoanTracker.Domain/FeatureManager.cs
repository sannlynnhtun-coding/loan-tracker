using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace LoanTracker.Domain;

public static class FeatureManager
{
    public static void AddDomain(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<CreateCustomerService>();
        builder.Services.AddScoped<GetCustomerService>();
        builder.Services.AddScoped<UpdateCustomerService>();
        builder.Services.AddScoped<DeleteCustomerService>();
    }
}