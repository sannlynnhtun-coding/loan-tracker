namespace LoanTracker.Api.Endpoints
{
    public static class PaymentEndpoint
    {
        public static IEndpointRouteBuilder UsePaymentEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/payment", async (CreatePaymentRequest request, PaymentService paymentService) =>
            {
                await paymentService.CreatePayment(request.Amount, request.PaymentDate, request.LoanId);
                return Results.Ok();
            })
            .WithName("CreatePayment")
            .WithOpenApi();

            return app;
        }
    }


}
