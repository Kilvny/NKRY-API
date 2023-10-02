using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NKRY_API.Helpers
{
    public static class FinanceHelper
    {
        public static decimal CalculateTotalSalary(int deliveriesMade,decimal deliveryRate, decimal baseSalary)
        {
            return (deliveriesMade * deliveryRate) + baseSalary;
        }
    }
}