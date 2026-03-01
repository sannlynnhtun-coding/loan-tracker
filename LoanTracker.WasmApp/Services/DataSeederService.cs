using LoanTracker.Shared.Models;
using System.Text.Json;

namespace LoanTracker.WasmApp.Services;

public class DataSeederService
{
    private readonly IndexedDbService _db;

    public DataSeederService(IndexedDbService db)
    {
        _db = db;
    }

    public async Task<bool> IsDatabaseEmpty()
    {
        var customers = await _db.GetAllCustomers();
        return customers.Count == 0;
    }

    public async Task SeedData(Action<int, string>? onProgress = null)
    {
        // Step 1: Purge
        onProgress?.Invoke(15, "Deconstructing Repository");
        await _db.ClearAll();

        // Step 2: Products
        onProgress?.Invoke(30, "Seeding Product Catalog");
        var loanTypes = new List<LoanTypeResponse>
        {
            new() { LoanTypeId = 1, LoanTypeName = "SME Core Capital", BurmeseLoanTypeName = "အသေးစား၊ အလတ်စား လုပ်ငန်းချေးငွေ", Description = "Enterprise-grade SME support." },
            new() { LoanTypeId = 2, LoanTypeName = "Equity Flex Credit", BurmeseLoanTypeName = "ရွှေပေါင်နှံချေးငွေ", Description = "Asset-backed liquidity." },
            new() { LoanTypeId = 3, LoanTypeName = "Agri-Harvest Fund", BurmeseLoanTypeName = "စိုက်ပျိုးရေး အထောက်အကူပြုချေးငွေ", Description = "Seasonal crop cycle financing." }
        };
        foreach (var type in loanTypes) await _db.AddLoanType(type);

        // Step 3: Customers
        onProgress?.Invoke(45, "Mapping Verified Borrowers");
        var customers = new List<CustomerResponse>
        {
            new() { CustomerId = 1, CustomerName = "Daw Hla Hla", Nrc = "12/MAGANA(N)123456", MobileNo = "09456789012", Address = "Yangon" },
            new() { CustomerId = 2, CustomerName = "U Ba Kyaw", Nrc = "9/MAKANA(N)987654", MobileNo = "09123456789", Address = "Mandalay" },
            new() { CustomerId = 3, CustomerName = "Maung Maung", Nrc = "7/PAMANA(N)112233", MobileNo = "09777888999", Address = "Naypyidaw" },
            new() { CustomerId = 4, CustomerName = "Daw Nu Nu", Nrc = "5/KANANA(N)111111", MobileNo = "09111111111", Address = "Bago" }
        };
        foreach (var c in customers) await _db.AddCustomer(c);

        // Step 4: Loans
        onProgress?.Invoke(65, "Generating Active Portfolio");
        await _db.CreateLoan(new CustomerLoanRequest { CustomerId = 1, LoanTypeId = 1, PrincipalAmount = 10000000, InterestRate = 12, LoanTerm = 12, RepaymentFrequency = "Monthly", LoanStartDate = DateTime.Today.AddMonths(-2) });
        await _db.CreateLoan(new CustomerLoanRequest { CustomerId = 2, LoanTypeId = 2, PrincipalAmount = 5000000, InterestRate = 14, LoanTerm = 6, RepaymentFrequency = "Monthly", LoanStartDate = DateTime.Today });
        await _db.CreateLoan(new CustomerLoanRequest { CustomerId = 3, LoanTypeId = 3, PrincipalAmount = 2000000, InterestRate = 10, LoanTerm = 12, RepaymentFrequency = "Monthly", LoanStartDate = DateTime.Today.AddMonths(-1) });

        // Step 5: Repayments
        onProgress?.Invoke(85, "Simulating Repayment Logic");
        var allLoans = await _db.GetAllLoans();
        foreach (var loan in allLoans)
        {
            var schedules = await _db.GetSchedulesForLoan(loan.LoanId);
            if (loan.LoanStartDate < DateTime.Today)
            {
                var pastSchedules = schedules.Take(1).ToList();
                foreach (var sch in pastSchedules)
                {
                    var sid = sch.GetProperty("scheduleId").GetInt32();
                    var amt = sch.GetProperty("installmentAmount").GetDecimal();
                    await _db.PostPayment(sid, amt, loan.LoanStartDate.AddMonths(1));
                }
            }
        }

        // Step 6: Finalize
        onProgress?.Invoke(100, "Finalizing Logic Sync");
        await Task.Delay(500);
    }
}
