namespace LoanTracker.Api.Endpoints
{
    public static class LateFeeRuleEndpoint
    {
        public static IEndpointRouteBuilder UseLateFeeRuleEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPut("/api/latefeerule", async (UpdateLateFeeRequest request, PaymentService paymentService) =>
            {
                await paymentService.UpdateLateFeeRule(request.LoanId, request.NewLateFee);
                return Results.Ok();
            })
            .WithName("UpdateLateFeeRule")
            .WithOpenApi();

            return app;
        }
    }

}
