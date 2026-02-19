using LoanTracker.Mvc.Services;
using Microsoft.AspNetCore.Mvc;
using LoanTracker.Shared.Models;

namespace LoanTracker.Mvc.Controllers;

public class CustomerLoanController : Controller
{
    private readonly HttpClientService _httpClientService;

    public CustomerLoanController(HttpClientService httpClientService)
    {
        _httpClientService = httpClientService;
    }

    public async Task<IActionResult> Index()
    {
        var result = await _httpClientService.GetAsync<List<CustomerLoanResponse>>("api/CustomerLoan");
        return View(result?.Data ?? new List<CustomerLoanResponse>());
    }

    public async Task<IActionResult> Create()
    {
        var customersResult = await _httpClientService.GetAsync<List<CustomerResponse>>("api/Customer");
        var loanTypesResult = await _httpClientService.GetAsync<List<LoanTypeResponse>>("api/LoanType");
        
        ViewBag.Customers = customersResult?.Data ?? new List<CustomerResponse>();
        ViewBag.LoanTypes = loanTypesResult?.Data ?? new List<LoanTypeResponse>();
        
        return View(new CustomerLoanRequest { LoanStartDate = DateTime.Today });
    }

    [HttpPost]
    public async Task<IActionResult> Create(CustomerLoanRequest loan)
    {
        if (ModelState.IsValid)
        {
            await _httpClientService.PostAsync<CustomerLoanRequest, object>("api/CustomerLoan", loan);
            return RedirectToAction(nameof(Index));
        }
        
        var customersResult = await _httpClientService.GetAsync<List<CustomerResponse>>("api/Customer");
        var loanTypesResult = await _httpClientService.GetAsync<List<LoanTypeResponse>>("api/LoanType");
        
        ViewBag.Customers = customersResult?.Data ?? new List<CustomerResponse>();
        ViewBag.LoanTypes = loanTypesResult?.Data ?? new List<LoanTypeResponse>();
        
        return View(loan);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var result = await _httpClientService.GetAsync<CustomerLoanResponse>($"api/CustomerLoan/{id}");
        if (result == null || !result.IsSuccess || result.Data == null) return NotFound();
        return View(result.Data);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _httpClientService.DeleteAsync($"api/CustomerLoan/{id}");
        return RedirectToAction(nameof(Index));
    }
}
