namespace LoanTracker.Api.Endpoints;

public static class PaymentEndpoint
{
	public static IEndpointRouteBuilder UsePaymentEndpoint(this IEndpointRouteBuilder app)
	{

		app.MapGet("/api/payment", async () => { })
			.WithName("").WithOpenApi();

		return app;
	}
}
