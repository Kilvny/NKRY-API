using Microsoft.AspNetCore.Identity;
using NKRY_API.Domain.Entities;
using NKRY_API.Models;
using NKRY_API.ResourceParameters;
using NKRY_API.Utilities;
using static NKRY_API.Utilities.Constants;

namespace NKRY_API.Domain.Contracts
{
    public interface IUserRepository : IGenericRepository<User>
    {
        public IEnumerable<User> GetAll(UsersResourceParameters usersResourceParameters);

        public void UpdateUserPassword(User user);
        public Task<UserManagerResponse> CreateUserAsync(User user);
    }
}
