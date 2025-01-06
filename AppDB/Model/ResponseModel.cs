using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDB.Model;

public class ResponseModel
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; }
    public object Data { get; set; }
}
