using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDB.Model;

public class Customer
{
    public Guid CustomerID { get; set; } = Guid.NewGuid();
    public string Name { get; set; }
    public string Email { get; set; }
    public string NRC { get; set; }
    public string MobileNumber { get; set; }
    public string Address { get; set; }
}
