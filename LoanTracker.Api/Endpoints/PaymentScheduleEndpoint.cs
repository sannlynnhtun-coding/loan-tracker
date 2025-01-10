using LoanTracker.Domain;

namespace LoanTracker.Api.Endpoints;

public static class PaymentScheduleEndpoint
{
	public static IEndpointRouteBuilder UsePaymentScheduleEndpoint(this IEndpointRouteBuilder app)
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





        app.MapGet("/api/paymentschedule/monthly/{id}", (string loanId) =>
        {
            PaymentScheduleService paymentScheduleService = new PaymentScheduleService();
            var model = paymentScheduleService.GetMonthlyPaymentSchedules(loanId);
            return model != null ? Results.Ok(model) : Results.NotFound("Loan not found or invalid loan ID.");
        })
       .WithName("GetMonthlyPaymentSchedule")
       .WithOpenApi();
        return app;
	}


}
