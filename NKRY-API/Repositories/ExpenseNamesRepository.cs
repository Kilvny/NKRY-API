using NKRY_API.DataAccess.EFCore;
using NKRY_API.Domain.Entities;

namespace NKRY_API.Repositories
{
    public class ExpenseNamesRepository : GenericRepository<ExpenseNames>
    {
        public ExpenseNamesRepository(ApplicationContext applicationContext): base(applicationContext)
        {
            
        }

        public ExpenseNames GetById(int id) // TODO: change this id type to string
        {
            return _applicationContext.Set<ExpenseNames>()
                                      .Find(id);
        }
    }
}
