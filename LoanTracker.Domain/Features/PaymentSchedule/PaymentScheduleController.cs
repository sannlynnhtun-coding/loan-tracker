namespace LoanTracker.Domain.Features.PaymentSchedule;

[ApiController]
[Route("api/[controller]")]
public class PaymentScheduleController : ControllerBase
{
    private readonly PaymentScheduleService _paymentScheduleService;

    public PaymentScheduleController(PaymentScheduleService paymentScheduleService)
    {
        _paymentScheduleService = paymentScheduleService;
    }

    [HttpPost("{loanId}")]
    public async Task<IResult> GeneratePaymentSchedule(int loanId)
    {
        var result = await _paymentScheduleService.GeneratePaymentScheduleAsync(loanId);
        return result.Execute();
    }

    [HttpGet("upcoming/{loanId}")]
    public async Task<IResult> GetUpcomingPayments(int loanId)
    {
        var result = await _paymentScheduleService.GetUpcomingPaymentsAsync(loanId);
        return result.Execute();
    }

    [HttpPut("mark-paid/{scheduleId}")]
    public async Task<IResult> MarkPaymentAsCompleted(int scheduleId)
    {
        var result = await _paymentScheduleService.MarkPaymentAsCompletedAsync(scheduleId);
        return result.Execute();
    }
}