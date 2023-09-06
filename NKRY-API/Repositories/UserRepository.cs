using NKRY_API.DataAccess.EFCore;
using NKRY_API.Domain.Contracts;
using NKRY_API.Domain.Entities;
using static NKRY_API.Utilities.Constants;

namespace NKRY_API.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationContext applicationContext) 
            : base(applicationContext)
        {

        }

        public void UpdateUserPassword(User user)
        {

        }
        public UserRole GetUserRole(int id)
        {
            User user = _applicationContext.users.Find(id);
            return user.Role;
        }
        public IEnumerable<User> GetAll(string userDepartment)
        {
            IEnumerable<User> users = _applicationContext.Set<User>().ToList();
            
            if (string.IsNullOrWhiteSpace(userDepartment))
            {
                return users;
            }

            userDepartment = userDepartment.Trim();
            IEnumerable<User> filteredUsers = _applicationContext.users.Where(u => u.Department.DepartmentName == userDepartment).ToList();
            return filteredUsers;
            /*
             * Test this by setting user departments manually first
             * Implement Searching
             * **/



        }

    }
}
