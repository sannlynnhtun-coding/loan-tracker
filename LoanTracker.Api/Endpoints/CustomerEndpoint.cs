using LoanTracker.Domain;

namespace LoanTracker.Api.Endpoints;

public static class CustomerEndpoint
{
	public static IEndpointRouteBuilder UseCustomerEndpoint(this IEndpointRouteBuilder app)
	{
		CustomerService service = new CustomerService();

		app.MapGet("/api/customer/{id}", (string id) => {
			var model = service.GetCustomerById(id);
			return Results.Ok(model);
		})
			.WithName("GetCustomerById")
			.WithOpenApi();

		return app;
	}
}
