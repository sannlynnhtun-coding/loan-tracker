using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanTracker.Domain;

public class PaymentService
{
    private readonly AppDbContext _context;

    public PaymentService(AppDbContext context)
    {
        _context = context;
    }

    public async Task CreatePayment(decimal amount, DateTime paymentDate, string loanId)
    {
        var payment = new Payment
        {
            Amount = amount,
            PaymentDate = paymentDate,
            LoanId = loanId
        };

        _context.Payments.Add(payment);
        await _context.SaveChangesAsync();
    }
}
