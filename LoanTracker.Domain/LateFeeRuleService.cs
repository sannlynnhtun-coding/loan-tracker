using LoanTracker.Database.AppDbContext;
using LoanTracker.Database.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
}
