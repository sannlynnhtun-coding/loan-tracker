﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoanTracker.Database.Models
{
    public class YearlyPaymentScheduleModel
    {
        public int Year { get; set; }
        public decimal Principal { get; set; }
        public decimal Interest { get; set; }
        public decimal TotalPayment { get; set; }
        public decimal RemainingBalance { get; set; }
    }

}
