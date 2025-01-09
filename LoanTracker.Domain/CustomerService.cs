using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoanTracker.Database.AppDbContext;
using LoanTracker.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace LoanTracker.Domain;

public class CustomerService
{
    private readonly AppDbContext _db;

    public CustomerService()
    {
        _db = new AppDbContext();
    }

    public ResponseModel GetCustomerById(string id)
    {
        try
        {
            var customer = _db.Customer.AsNoTracking().FirstOrDefault(x => x.Id == id);
            if (customer is null)
            {
                return new ResponseModel { Message = "User not found!" };
            }

            return new ResponseModel { IsSuccess = true, Message = "Success", Data = customer };
        }
        catch(Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }
}
