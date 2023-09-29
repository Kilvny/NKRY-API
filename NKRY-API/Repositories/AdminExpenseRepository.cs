using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NKRY_API.DataAccess.EFCore;
using NKRY_API.Domain.Contracts;
using NKRY_API.Domain.Entities;
using NKRY_API.ResourceParameters;

namespace NKRY_API.Repositories
{
    public class AdminExpenseRepository : GenericRepository<AdminExpense>, IAdminExpenseRepository
    {
        public AdminExpenseRepository(ApplicationContext applicationContext) : base(applicationContext)
        {
            
        }

        public IEnumerable<AdminExpense> GetAll(AdminsResourceParameters adminsResourceParameters) 
        {
            if (adminsResourceParameters == null)
            {
                throw new ArgumentNullException(nameof(adminsResourceParameters));
            }
            string adminId = adminsResourceParameters.AdminId;
            string dateFrom = adminsResourceParameters.DateFrom;
            string searchQuery = adminsResourceParameters.SearchQuery;

            bool isAdminIdNull = string.IsNullOrWhiteSpace(adminId);
            bool isSearchQueryNull = string.IsNullOrWhiteSpace(searchQuery);
            bool isDateFromNull = string.IsNullOrWhiteSpace(dateFrom);

            if (isAdminIdNull && isDateFromNull && isSearchQueryNull)
            {
                return this.GetAll();
            }
            // it's good practice to use differed execution so we cast users object as IQueryable<User> type 
            var adminExpenses = _applicationContext.users as IQueryable<AdminExpense>;
            if (!isAdminIdNull)
            {
                throw new NotImplementedException();
                // adminId = adminId.Trim();
                // Guid ag;
                // ag = Guid.ParseExact(adminId, "A");
                // var filteredUsers = adminExpenses.Where(a => a.AdminId == ag);
                // adminExpenses = filteredUsers;
            }

            if (!isDateFromNull)
            {
                throw new NotImplementedException();
                // dateFrom = dateFrom.Trim();
                // var filteredUsers = adminExpenses.Where(u => u.Department.DepartmentName == dateFrom);
                // adminExpenses = filteredUsers;
            }

            if (!isSearchQueryNull)
            {
                throw new NotImplementedException();
                // searchQuery = searchQuery.Trim();
                // var searchResult = adminExpenses.Where(u => u.Address.Contains(searchQuery)
                // || u.Email.Contains(searchQuery));
                // adminExpenses = searchResult;
            }
            // now we execute after the filtration done on the query first
            return adminExpenses.ToList();
        }
    }
}