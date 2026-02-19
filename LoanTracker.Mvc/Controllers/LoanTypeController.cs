using LoanTracker.Mvc.Services;
using Microsoft.AspNetCore.Mvc;
using LoanTracker.Shared.Models;

namespace LoanTracker.Mvc.Controllers;

public class LoanTypeController : Controller
{
    private readonly HttpClientService _httpClientService;

    public LoanTypeController(HttpClientService httpClientService)
    {
        _httpClientService = httpClientService;
    }

    public async Task<IActionResult> Index()
    {
        var result = await _httpClientService.GetAsync<List<LoanTypeResponse>>("api/LoanType");
        return View(result?.Data ?? new List<LoanTypeResponse>());
    }

    public IActionResult Create()
    {
        return View(new LoanTypeRequest());
    }

    [HttpPost]
    public async Task<IActionResult> Create(LoanTypeRequest loanType)
    {
        if (ModelState.IsValid)
        {
            await _httpClientService.PostAsync<LoanTypeRequest, object>("api/LoanType", loanType);
            return RedirectToAction(nameof(Index));
        }
        return View(loanType);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var result = await _httpClientService.GetAsync<LoanTypeResponse>($"api/LoanType/{id}");
        if (result == null || !result.IsSuccess || result.Data == null) return NotFound();

        var loanType = result.Data;
        var request = new LoanTypeRequest
        {
            LoanTypeId = loanType.LoanTypeId,
            LoanTypeName = loanType.LoanTypeName,
            Description = loanType.Description,
            BurmeseLoanTypeName = loanType.BurmeseLoanTypeName,
            BurmeseDescription = loanType.BurmeseDescription
        };

        return View(request);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, LoanTypeRequest loanType)
    {
        if (ModelState.IsValid)
        {
            await _httpClientService.PutAsync<LoanTypeRequest, object>($"api/LoanType/{id}", loanType);
            return RedirectToAction(nameof(Index));
        }
        return View(loanType);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var result = await _httpClientService.GetAsync<LoanTypeResponse>($"api/LoanType/{id}");
        if (result == null || !result.IsSuccess || result.Data == null) return NotFound();
        return View(result.Data);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _httpClientService.DeleteAsync($"api/LoanType/{id}");
        return RedirectToAction(nameof(Index));
    }
}
