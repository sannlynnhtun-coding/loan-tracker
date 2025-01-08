namespace LoanTracker.Api.Endpoints;

public static class CustomerEndpoint
{
	public static IEndpointRouteBuilder UseCustomerEndpoint(this IEndpointRouteBuilder app)
	{

		app.MapGet("/api/customer", async () => { })
			.WithName("").WithOpenApi();

		return app;
	}
}
