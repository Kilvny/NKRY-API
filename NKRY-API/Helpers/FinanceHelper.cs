using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NKRY_API.Helpers
{
    public static class FinanceHelper
    {
        public static decimal CalculateTotalSalary(int deliveriesMade, decimal deliveryRate, decimal currSalary)
        {
            if (deliveriesMade == null && deliveryRate == null && currSalary == null)
                return 0;
            return (deliveriesMade * deliveryRate) + currSalary;
        }
    }
}