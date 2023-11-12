using NKRY_API.DataAccess.EFCore;
using NKRY_API.Domain.Entities;

namespace NKRY_API.Repositories
{
    public class PersonalDetailsRepository : GenericRepository<PersonalDetails>
    {
        public PersonalDetailsRepository(ApplicationContext applicationContext) : base(applicationContext)
        {

        }

        public PersonalDetails GetByEmployeeId(Guid employeeId)
        {
            PersonalDetails detailsOfEmployee = _applicationContext
                .personalDetails
                .Where(p => p.EmployeeId == employeeId)
                .FirstOrDefault();
            return detailsOfEmployee;
        }
    }
}
