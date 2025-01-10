using LoanTracker.Database.AppDbContext;
using LoanTracker.Database.Models;
using LoanTracker.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace LoanTracker.Domain;

public class LateFeeRuleService
{
    private readonly AppDbContext _context;

    public LateFeeRuleService()
    {
        _context = new AppDbContext();
    }
    public ResponseModel CreateLateFee(LateFeeRuleModel lateFee)
    {
        try
        {
            _context.LateFee.Add(lateFee);
            _context.SaveChanges();
            return new ResponseModel { IsSuccess = true, Message = "Late Fee Rule created successfully." };
        }
        catch (Exception ex)
        {
            return new ResponseModel { IsSuccess = false, Message = ex.Message };
        }
    }
    public void UpdateLateFeeRule(string id, int minDaysOverdue, int maxDaysOverdue, decimal lateFeeAmount)
    {
        var item = _context.LateFee.AsNoTracking().FirstOrDefault(x => x.Id == id);
        if (item == null)
        {
            Console.WriteLine("No data found.");
            return;
        }

        item.MinDaysOverdue = minDaysOverdue;
        item.MaxDaysOverdue = maxDaysOverdue;
        item.LateFeeAmount = lateFeeAmount;

        _context.Entry(item).State = EntityState.Modified;
        var result = _context.SaveChanges();

        string message = result > 0 ? "Updating Successful." : "Updating Failed.";
        Console.WriteLine(message);
    }

    public ResponseModel DeleteLateFeeById(string id)
    {
        var lateFeeRule = _context.LateFee.AsNoTracking().FirstOrDefault(x => x.Id == id);
        if (lateFeeRule is null)
        {
            return new ResponseModel()
            {
                IsSuccess = false,
                Message = "No data Found"
            };
        }
        _context.Entry(lateFeeRule).State = EntityState.Deleted;
        var result = _context.SaveChanges();
        string message = result > 0 ? "Deleting late fee rule successful." : " Deleting late fee rule Failed.";
        ResponseModel responseModel = new ResponseModel();
        responseModel.IsSuccess = result > 0;
        responseModel.Message = message;
        responseModel.Data = lateFeeRule;
        return responseModel;

    }

}
