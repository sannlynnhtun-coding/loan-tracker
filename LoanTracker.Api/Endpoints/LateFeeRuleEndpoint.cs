using LoanTracker.Database.Models;
using LoanTracker.Domain;

namespace LoanTracker.Api.Endpoints
{
    public static class LateFeeRuleEndpoint
    {
        public static IEndpointRouteBuilder UseLateFeeRuleEndpoint(this IEndpointRouteBuilder app)
        {
            LateFeeRuleService lateFeeService = new LateFeeRuleService();
            app.MapPut("/api/latefeerule", async (UpdateLateFeeRequest request, PaymentService paymentService) =>
            {
                await paymentService.UpdateLateFeeRule(request.LoanId, request.NewLateFee);
                return Results.Ok();
            })
            .WithName("UpdateLateFeeRule")
            .WithOpenApi();

            app.MapPost("/api/latefeerule", (LateFeeRuleModel lateFeeRule) =>
             {
                 var model = lateFeeService.CreateLateFee(lateFeeRule);
                 return Results.Ok(model);
             })
              .WithName("CreateLateFeeRule")
             .WithOpenApi();


            app.MapDelete("/api/latefeerule/{id}", (string id) =>
            {
                var model = lateFeeService.DeleteLateFeeById(id);
                return Results.Ok(model);
            })
           .WithName("DeleteLateFeeRule")
          .WithOpenApi();

			app.MapGet("/api/latefeerule/", async () =>
			{
				LateFeeRuleService lateFeeRuleService = new();
				var responseModel = await lateFeeRuleService.GetLateFeeRules();
				if (!responseModel.IsSuccess) return Results.Json(responseModel, statusCode: 500);
				return Results.Ok(responseModel);
			})
            .WithName("GetLateFeeRules")
            .WithOpenApi();

			return app;
        }
    }

}
