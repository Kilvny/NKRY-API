using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NKRY_API.ResourceParameters
{
    public class EmployeeFinanceResourceParams
    {
        private int? _year;
        private int? _month;

        public int? Year
        {
            get => _year ?? DateTime.Now.Year;
            set => _year = value;
        }
        public int? Month 
        {
            get => _month ?? DateTime.Now.Month;
            set => _month = value;
        }
    }
}