using Microsoft.AspNetCore.Identity;
using NKRY_API.Domain.Entities;
using NKRY_API.ResourceParameters;
using static NKRY_API.Utilities.Constants;

namespace NKRY_API.Domain.Contracts
{
    public interface IUserRepository : IGenericRepository<User>
    {
        IEnumerable<User> GetAll(UsersResourceParameters usersResourceParameters);
        void UpdateUserPassword(User user);
        public UserRole GetUserRole(int id);
    }
}
