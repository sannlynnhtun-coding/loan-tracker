using LoanTracker.Mvc.Services;
using Microsoft.AspNetCore.Mvc;
using LoanTracker.Shared.Models;

namespace LoanTracker.Mvc.Controllers;

public class CustomerController : Controller
{
    private readonly HttpClientService _httpClientService;

    public CustomerController(HttpClientService httpClientService)
    {
        _httpClientService = httpClientService;
    }

    public async Task<IActionResult> Index()
    {
        var result = await _httpClientService.GetAsync<List<CustomerResponse>>("api/Customer");
        return View(result?.Data ?? new List<CustomerResponse>());
    }

    public IActionResult Create()
    {
        return View(new CustomerRequest());
    }

    [HttpPost]
    public async Task<IActionResult> Create(CustomerRequest customer)
    {
        if (ModelState.IsValid)
        {
            await _httpClientService.PostAsync<CustomerRequest, object>("api/Customer", customer);
            return RedirectToAction(nameof(Index));
        }
        return View(customer);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var result = await _httpClientService.GetAsync<CustomerResponse>($"api/Customer/{id}");
        if (result == null || !result.IsSuccess || result.Data == null) return NotFound();

        var customer = result.Data;
        var request = new CustomerRequest
        {
            CustomerId = customer.CustomerId,
            CustomerName = customer.CustomerName,
            Nrc = customer.Nrc,
            MobileNo = customer.MobileNo,
            Address = customer.Address,
            CreatedDate = customer.CreatedDate
        };

        return View(request);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, CustomerRequest customer)
    {
        if (ModelState.IsValid)
        {
            await _httpClientService.PutAsync<CustomerRequest, object>($"api/Customer/{id}", customer);
            return RedirectToAction(nameof(Index));
        }
        return View(customer);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var result = await _httpClientService.GetAsync<CustomerResponse>($"api/Customer/{id}");
        if (result == null || !result.IsSuccess || result.Data == null) return NotFound();
        return View(result.Data);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _httpClientService.DeleteAsync($"api/Customer/{id}");
        return RedirectToAction(nameof(Index));
    }
}
