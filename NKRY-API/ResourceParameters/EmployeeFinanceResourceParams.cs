using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NKRY_API.ResourceParameters
{
    public class EmployeeFinanceResourceParams
    {
        public int? Year
        {
            get => Year ?? DateTime.Now.Year;
            set => Year = value;
        }
        public int? Month 
        {
            get => Month ?? DateTime.Now.Month;
            set => Month = value;
        }
    }
}