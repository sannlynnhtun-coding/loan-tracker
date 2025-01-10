using LoanTracker.Database.Models;
using LoanTracker.Domain;

namespace LoanTracker.Api.Endpoints;

public static class CustomerEndpoint
{
    public static IEndpointRouteBuilder UseCustomerEndpoint(this IEndpointRouteBuilder app)
    {

        app.MapGet("/api/customer/", async () =>
        {
            CustomerService customerService = new();
            var responseModel = await customerService.GetCustomers();
            if (!responseModel.IsSuccess) return Results.Json(responseModel, statusCode: 500);
            return Results.Ok(responseModel);
        })
        .WithName("GetCustomers")
        .WithOpenApi();

        app.MapGet("/api/customer/{id}", (string id) =>
        {
            CustomerService service = new CustomerService();
            var model = service.GetCustomerById(id);
            return Results.Ok(model);
        })
            .WithName("GetCustomerById")
            .WithOpenApi();


        app.MapPost("/api/customers", (CustomerModel requestModel) =>
        {
            CustomerService service = new CustomerService();
            var model = service.CreateCustomer(requestModel);
            return Results.Ok(model);
        })
            .WithName("CreateCustomers")
            .WithOpenApi();

        return app;
    }
}
