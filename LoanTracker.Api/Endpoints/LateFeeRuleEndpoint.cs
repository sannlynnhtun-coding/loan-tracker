namespace LoanTracker.Api.Endpoints;

public static class LateFeeRuleEndpoint
{
	public static IEndpointRouteBuilder UseLateFeeRuleEndpoint(this IEndpointRouteBuilder app)
	{

		app.MapGet("/api/latefeerule", async () => { })
			.WithName("").WithOpenApi();

		return app;
	}
}
