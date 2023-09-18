using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NKRY_API.DataAccess.EFCore;
using NKRY_API.Domain.Contracts;
using NKRY_API.Domain.Entities;
using NKRY_API.Models;
using NKRY_API.ResourceParameters;
using NKRY_API.Utilities;
using static NKRY_API.Utilities.Constants;

namespace NKRY_API.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly ILogger _logger;
        private readonly UserManager<User> _userManager;

        public UserRepository(ApplicationContext applicationContext, ILogger logger, UserManager<User> userManager) 
            : base(applicationContext)
        {
            _logger = logger;
            _userManager = userManager;
        }

        public void UpdateUserPassword(User user)
        {

        }

        public new IEnumerable<User> GetAllNow()
        {
            var users =  _userManager.Users.ToList();
            

            return users;
        }

        public IEnumerable<User> GetAll(UsersResourceParameters usersResourceParameters)
        {
            if (usersResourceParameters == null)
            {
                throw new ArgumentNullException(nameof(usersResourceParameters));
            }
            string userDepartment = usersResourceParameters.UserDepartment;
            string searchQuery = usersResourceParameters.SearchQuery;

            bool isUserDepartmentNull = string.IsNullOrWhiteSpace(userDepartment);
            bool isSearchQueryNull = string.IsNullOrWhiteSpace(searchQuery);

            if (isUserDepartmentNull && isSearchQueryNull)
            {
                return GetAll();
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

        public async Task<UserManagerResponse> CreateUserAsync(User user)
        {
            if (user == null)
            {
                throw new NullReferenceException("user  is null");
            }


            var result = await _userManager.CreateAsync(user, user.Password);

            if (!result.Succeeded)
            {
                return new UserManagerResponse
                {
                    Message = "Error while creating the user",
                    StatusCode = 400,
                    IsSuccess = false,
                    Errors = result.Errors.Select(err => err.Description)
                };
            }

            return new UserManagerResponse
            {
                Message = "User Created Successfully",
                StatusCode = 201,
                IsSuccess = true,
            };
        }

    }
}
