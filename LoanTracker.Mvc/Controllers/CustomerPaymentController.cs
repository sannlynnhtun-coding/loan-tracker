using LoanTracker.Mvc.Services;
using Microsoft.AspNetCore.Mvc;
using LoanTracker.Shared.Models;

namespace LoanTracker.Mvc.Controllers;

public class CustomerPaymentController : Controller
{
    private readonly HttpClientService _httpClientService;

    public CustomerPaymentController(HttpClientService httpClientService)
    {
        _httpClientService = httpClientService;
    }

    public async Task<IActionResult> Index()
    {
        var result = await _httpClientService.GetAsync<List<CustomerPaymentResponse>>("api/CustomerPayment");
        return View(result?.Data ?? new List<CustomerPaymentResponse>());
    }

    public IActionResult Create()
    {
        return View(new CustomerPaymentRequest { PaymentDate = DateTime.Today });
    }

    [HttpPost]
    public async Task<IActionResult> Create(CustomerPaymentRequest payment)
    {
        if (ModelState.IsValid)
        {
            await _httpClientService.PostAsync<CustomerPaymentRequest, object>("api/CustomerPayment", payment);
            return RedirectToAction(nameof(Index));
        }
        return View(payment);
    }
}
