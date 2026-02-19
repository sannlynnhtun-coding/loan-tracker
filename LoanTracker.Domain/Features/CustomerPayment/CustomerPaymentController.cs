using LoanTracker.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace LoanTracker.Domain.Features.CustomerPayment;

[ApiController]
[Route("api/[controller]")]
public class CustomerPaymentController : ControllerBase
{
    private readonly CustomerPaymentService _paymentService;

    public CustomerPaymentController(CustomerPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpPost]
    public async Task<IResult> AddPayment([FromBody] CustomerPaymentRequest payment)
    {
        var result = await _paymentService.AddPaymentAsync(payment);
        return result.Execute();
    }

    [HttpGet("{id}")]
    public async Task<IResult> GetPayment(int id)
    {
        var result = await _paymentService.GetPaymentByIdAsync(id);
        return result.Execute();
    }
}
