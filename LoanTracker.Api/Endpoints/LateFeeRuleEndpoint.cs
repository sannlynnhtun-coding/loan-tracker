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
            return app;
        }
    }

}
