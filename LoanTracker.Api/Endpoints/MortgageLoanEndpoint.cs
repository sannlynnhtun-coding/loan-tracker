namespace LoanTracker.Api.Endpoints;

public static class MortgageLoanEndpoint
{
	public static IEndpointRouteBuilder UseMortgageLoanEndpoint(this IEndpointRouteBuilder app)
	{

		app.MapGet("/api/mortgageloan", async () => { })
			.WithName("").WithOpenApi();

		return app;
	}
}
