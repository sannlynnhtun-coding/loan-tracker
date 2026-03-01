using Microsoft.JSInterop;
using LoanTracker.Shared.Models;
using System.Text.Json;

namespace LoanTracker.WasmApp.Services;

public class IndexedDbService
{
    private readonly IJSRuntime _jsRuntime;

    public IndexedDbService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    #region Generic Operations
    public async Task ClearAll()
    {
        await _jsRuntime.InvokeVoidAsync("window.clearAll");
    }
    #endregion

    #region Customers
    public async Task<List<CustomerResponse>> GetAllCustomers() =>
        await _jsRuntime.InvokeAsync<List<CustomerResponse>>("dbInstance.customers.toArray");

    public async Task AddCustomer(CustomerResponse customer)
    {
        if (customer.CustomerId == 0)
        {
            await _jsRuntime.InvokeVoidAsync("dbInstance.customers.add", new 
            { 
                customer.CustomerName, 
                customer.Nrc, 
                customer.MobileNo, 
                customer.Address, 
                customer.CreatedDate 
            });
        }
        else
        {
            await _jsRuntime.InvokeVoidAsync("dbInstance.customers.add", customer);
        }
    }

    public async Task UpdateCustomer(CustomerResponse customer) =>
        await _jsRuntime.InvokeVoidAsync("dbInstance.customers.put", customer);

    public async Task DeleteCustomer(int id) =>
        await _jsRuntime.InvokeVoidAsync("dbInstance.customers.delete", id);
    #endregion

    #region Loan Types
    public async Task<List<LoanTypeResponse>> GetAllLoanTypes() =>
        await _jsRuntime.InvokeAsync<List<LoanTypeResponse>>("dbInstance.loanTypes.toArray");

    public async Task AddLoanType(LoanTypeResponse type)
    {
        if (type.LoanTypeId == 0)
        {
            await _jsRuntime.InvokeVoidAsync("dbInstance.loanTypes.add", new 
            { 
                type.LoanTypeName, 
                type.BurmeseLoanTypeName, 
                type.Description 
            });
        }
        else
        {
            await _jsRuntime.InvokeVoidAsync("dbInstance.loanTypes.add", type);
        }
    }

    public async Task UpdateLoanType(LoanTypeResponse type) =>
        await _jsRuntime.InvokeVoidAsync("dbInstance.loanTypes.put", type);

    public async Task DeleteLoanType(int id) =>
        await _jsRuntime.InvokeVoidAsync("dbInstance.loanTypes.delete", id);
    #endregion

    #region Loans & Schedules (Business Logic)
    public async Task<List<CustomerLoanResponse>> GetAllLoans() =>
        await _jsRuntime.InvokeAsync<List<CustomerLoanResponse>>("dbInstance.loans.toArray");

    public async Task CreateLoan(CustomerLoanRequest request)
    {
        // 1. Calculate Total Amount
        var totalAmount = request.PrincipalAmount + (request.PrincipalAmount * (request.InterestRate / 100) * (request.LoanTerm / 12.0m));

        // 2. Create Loan Record
        var loan = new CustomerLoanResponse
        {
            CustomerId = request.CustomerId,
            LoanTypeId = request.LoanTypeId,
            PrincipalAmount = request.PrincipalAmount,
            InterestRate = request.InterestRate,
            LoanTerm = request.LoanTerm,
            LoanStartDate = request.LoanStartDate,
            RepaymentFrequency = request.RepaymentFrequency ?? "Monthly",
            Status = "Active",
            TotalAmount = totalAmount
        };

        // Use a dynamic object to avoid sending LoanId: 0 which conflicts with auto-increment
        var loanData = new
        {
            loan.CustomerId,
            loan.LoanTypeId,
            loan.PrincipalAmount,
            loan.InterestRate,
            loan.LoanTerm,
            loan.LoanStartDate,
            loan.RepaymentFrequency,
            loan.Status,
            loan.TotalAmount
        };

        var loanId = await _jsRuntime.InvokeAsync<int>("dbInstance.loans.add", loanData);
        loan.LoanId = loanId;

        // 3. Generate Schedules (Logic from CustomerLoanService.cs)
        decimal monthlyInterestRate = request.InterestRate / 100 / 12;
        int totalPayments = request.LoanTerm * (loan.RepaymentFrequency == "Monthly" ? 12 : 1);
        decimal monthlyPayment = request.PrincipalAmount * (monthlyInterestRate / (1 - (decimal)Math.Pow(1 + (double)monthlyInterestRate, -totalPayments)));

        var schedules = new List<object>();
        var currentPrincipal = request.PrincipalAmount;
        var startDate = DateOnly.FromDateTime(request.LoanStartDate);

        for (int i = 1; i <= totalPayments; i++)
        {
            var dueDate = startDate.AddMonths(i);
            var interestComponent = currentPrincipal * monthlyInterestRate;
            var principalComponent = monthlyPayment - interestComponent;
            currentPrincipal -= principalComponent;

            schedules.Add(new
            {
                loanId = loanId,
                dueDate = dueDate.ToString("yyyy-MM-dd"),
                installmentAmount = monthlyPayment,
                principalComponent = principalComponent,
                interestComponent = interestComponent,
                remainingBalance = currentPrincipal,
                status = "Pending"
            });
        }

        await _jsRuntime.InvokeVoidAsync("dbInstance.paymentSchedules.bulkAdd", schedules);
    }

    public async Task<List<JsonElement>> GetSchedulesForLoan(int loanId)
    {
        return await _jsRuntime.InvokeAsync<List<JsonElement>>("getSchedulesByLoanId", loanId);
    }

    public async Task PostPayment(int scheduleId, decimal amount, DateTime date)
    {
        // 1. Record the Payment
        var payment = new
        {
            scheduleId = scheduleId,
            amount = amount,
            paymentDate = date.ToString("yyyy-MM-dd"),
            status = "Success"
        };
        await _jsRuntime.InvokeVoidAsync("dbInstance.payments.add", payment);

        // 2. Update Schedule Status
        var schedule = await _jsRuntime.InvokeAsync<JsonElement>("dbInstance.paymentSchedules.get", scheduleId);
        if (schedule.ValueKind != JsonValueKind.Undefined)
        {
            // Simple put to update the status
            await _jsRuntime.InvokeVoidAsync("dbInstance.paymentSchedules.update", scheduleId, new { status = "Paid" });
        }
    }
    #endregion
}
