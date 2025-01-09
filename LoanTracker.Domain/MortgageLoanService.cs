using LoanTracker.Database.AppDbContext;
using LoanTracker.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace LoanTracker.Domain;

public class MortgageLoanService
{
    private readonly AppDbContext _db;

    public MortgageLoanService()
    {
        _db = new AppDbContext();
    }

    public ResponseModel CreateMortgageLoan(LoanDTO requestModel)
    {
        try
        {
            var customer = _db.Customer.AsNoTracking().FirstOrDefault(x => x.Id == requestModel.CustomerId);
            if (customer is null)
            {
                return new ResponseModel { Message = "User not found!" };
            }

            var principle = requestModel.LoanAmount - requestModel.DownPayment;
            var monthlyInterestRate = (double)requestModel.InterestRate / 12;
            var totalMonths = requestModel.LoanTerm * 12;
            var monthlyPayment = principle * (decimal)(monthlyInterestRate * Math.Pow(1 + monthlyInterestRate, totalMonths)) /
                                     (decimal)(Math.Pow(1 + monthlyInterestRate, totalMonths) - 1);
            var totalRepayment = principle + (principle * (requestModel.InterestRate / 100) * principle);

            var model = new MortgageLoanModel
            {
                CustomerId = requestModel.CustomerId,
                LoanAmount = requestModel.LoanAmount,
                InterestRate = requestModel.InterestRate,
                LoanTerm = requestModel.LoanTerm,
                MonthlyPayment = monthlyPayment,
                DownPayment = requestModel.DownPayment,
                TotalRepayment = totalRepayment
            };

            _db.LoanDetails.Add(model);
            var result = _db.SaveChanges();

            string message = result > 0 ? "Successfully Create Loan." : "Fail to Create Loan!";
            ResponseModel response = new ResponseModel();
            response.IsSuccess = result > 0;
            response.Message = message;

            return response;
        }
        catch(Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }
}
