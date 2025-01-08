namespace LoanTracker.Api.Endpoints;

public static class PaymentScheduleEndpoint
{
	public static IEndpointRouteBuilder UseCustomerEndpoint(this IEndpointRouteBuilder app)
	{

		app.MapGet("/api/paymentschedule", async () => { })
			.WithName("").WithOpenApi();

		return app;
	}
}
