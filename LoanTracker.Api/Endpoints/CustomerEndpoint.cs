using LoanTracker.Domain;

namespace LoanTracker.Api.Endpoints;

public static class CustomerEndpoint
{
	public static IEndpointRouteBuilder UseCustomerEndpoint(this IEndpointRouteBuilder app)
	{

		app.MapGet("/api/customer/{id}", (string id) => {
			CustomerService service = new CustomerService();
			var model = service.GetCustomerById(id);
			return Results.Ok(model);
		})
			.WithName("GetCustomerById")
			.WithOpenApi();

		return app;
	}
}
