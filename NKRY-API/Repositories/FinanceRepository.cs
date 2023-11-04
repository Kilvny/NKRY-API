using NKRY_API.DataAccess.EFCore;
using NKRY_API.Domain.Entities;

namespace NKRY_API.Repositories
{
    public class FinanceRepository : GenericRepository<FixedFinance>
    {
        public FinanceRepository(ApplicationContext applicationContext) : base(applicationContext)
        {

        }

        public FixedFinance GetFinanceByEmployeeId(Guid employeeId)
        {
            FixedFinance res = _applicationContext.finances.Where(f => f.EmployeeId == employeeId).FirstOrDefault();
            return res;
        }

        public FixedFinance UpdateFinanceOfEmployee(FixedFinance finance)
        {
            var financeOfEmployee = GetFinanceByEmployeeId((Guid)finance.EmployeeId);
            if (financeOfEmployee != null)
            {
                _applicationContext.finances.Update(financeOfEmployee);
            }
            return financeOfEmployee;
        }
    }

}