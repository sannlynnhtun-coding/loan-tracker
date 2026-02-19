using LoanTracker.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace LoanTracker.Application.Services;

[ApiController]
[Route("api/[controller]")]
public class LoanTypeController : ControllerBase
{
    private readonly LoanTypeService _loanTypeService;

    public LoanTypeController(LoanTypeService loanTypeService)
    {
        _loanTypeService = loanTypeService;
    }

    [HttpPost]
    public async Task<IResult> AddLoanType([FromBody] LoanTypeRequest request)
    {
        var result = await _loanTypeService.AddLoanTypeAsync(request);
        return result.Execute();
    }

    [HttpGet("{id}")]
    public async Task<IResult> GetLoanType(int id)
    {
        var result = await _loanTypeService.GetLoanTypeByIdAsync(id);
        return result.Execute();
    }

    [HttpGet]
    public async Task<IResult> GetAllLoanTypes()
    {
        var result = await _loanTypeService.GetAllLoanTypesAsync();
        return result.Execute();
    }

    [HttpPut("{id}")]
    public async Task<IResult> UpdateLoanType(int id, [FromBody] LoanTypeRequest request)
    {
        request.LoanTypeId = id;
        var result = await _loanTypeService.UpdateLoanTypeAsync(request);
        return result.Execute();
    }

    [HttpDelete("{id}")]
    public async Task<IResult> DeleteLoanType(int id)
    {
        var result = await _loanTypeService.DeleteLoanTypeAsync(id);
        return result.Execute();
    }
}