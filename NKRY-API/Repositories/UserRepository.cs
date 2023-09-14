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
        public IEnumerable<User> GetAll(string userDepartment, string searchQuery)
        {
            bool isUserDepartmentNull = string.IsNullOrWhiteSpace(userDepartment);
            bool isSearchQueryNull = string.IsNullOrWhiteSpace(searchQuery);

            if (isUserDepartmentNull && isSearchQueryNull)
            {
                IEnumerable<User> allUsers = _applicationContext.Set<User>().ToList();
                return allUsers;
            }
            // it's good practice to use differed execution so we cast users object as IQueryable<User> type 
            var users = _applicationContext.users as IQueryable<User>;
            if (!isUserDepartmentNull)
            {
                userDepartment = userDepartment.Trim();
                var filteredUsers = users.Where(u => u.Department.DepartmentName == userDepartment);
                users = filteredUsers;
            }

            if (!isSearchQueryNull)
            {
                searchQuery = searchQuery.Trim();
                var searchResult = users.Where(u => u.Address.Contains(searchQuery)
                || u.Email.Contains(searchQuery));
                users = searchResult;
            }
            // now we execute after the filtration done on the query first
            return users.ToList();

        }

    }
}
