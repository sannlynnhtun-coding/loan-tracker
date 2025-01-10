using LoanTracker.Database.AppDbContext;
using LoanTracker.Database.Models;
using LoanTracker.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace LoanTracker.Domain;

public class CustomerService
{
    private readonly AppDbContext _db;

    public CustomerService()
    {
        _db = new AppDbContext();
    }

    public async Task<ResponseModel> GetCustomers()
    {
        ResponseModel responseModel = new();

        try
        {
            var customers = await _db.Customer.ToListAsync();
            responseModel.IsSuccess = true;
            responseModel.Message = "Successfully retrieved.";
            responseModel.Data = customers;
            return responseModel;
        }
        catch (Exception ex)
        {
            responseModel.IsSuccess = false;
            responseModel.Message = ex.Message;
            return responseModel;
        }
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
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

    public ResponseModel CreateCustomer(CustomerModel requestModel)
    {
        try
        {
            _db.Customer.Add(requestModel);
            int result = _db.SaveChanges();
            var message = result > 0 ? "Saving Customer Successful " : "Saving Customer Failed";
            ResponseModel model = new ResponseModel();
            model.IsSuccess = result > 0;
            model.Message = message;

            return model;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }

    }
}
