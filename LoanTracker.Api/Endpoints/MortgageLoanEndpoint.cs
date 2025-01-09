using LoanTracker.Database.Models;
using LoanTracker.Domain;

namespace LoanTracker.Api.Endpoints;

public static class MortgageLoanEndpoint
{
	public static IEndpointRouteBuilder UseMortgageLoanEndpoint(this IEndpointRouteBuilder app)
	{
		MortgageLoanService service = new MortgageLoanService();

		app.MapPost("/api/mortgageLoan/CreateMortgageLoan", (LoanDTO requestModel) =>
		{
			var response =  service.CreateMortgageLoan(requestModel);
			return Results.Ok(response);
		})
			.WithName("CreateMortgageLoan")
			.WithOpenApi();

		return app;
	}
}
