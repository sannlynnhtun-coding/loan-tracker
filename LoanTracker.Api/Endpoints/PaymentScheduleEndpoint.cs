using LoanTracker.Domain;

namespace LoanTracker.Api.Endpoints;

public static class PaymentScheduleEndpoint
{
	public static IEndpointRouteBuilder UseCustomerEndpoint(this IEndpointRouteBuilder app)
	{

		app.MapGet("/api/paymentschedule", async () => { })
			.WithName("").WithOpenApi();
		app.MapGet("/api/paymentschedule/{id}", (string loanId) => 
		{ 
			PaymentScheduleService paymentScheduleService = new PaymentScheduleService();
            var model = paymentScheduleService.GetYearlyPaymentSchedule(loanId);	
			return Results.Ok(model);
        })
            .WithName("GetYearlyPaymentSchedule").WithOpenApi();
        return app;
	}
}
