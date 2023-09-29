namespace NKRY_API.Domain.Contracts
{
    /// <summary>
    /// The Unit of Work pattern is a design pattern used in software engineering
    /// to manage a group of operations that need to be performed as a single unit, 
    /// typically involving multiple repositories or data access operations. 
    /// The goal is to ensure that changes are committed or rolled back consistently.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository User { get; }
        IAdminExpenseRepository AdminExpense { get; }
        ICarRepository Car { get; }
        IEmployeeFinanceRepository EmployeeFinance { get; }
        IEmployeeRepository Employee { get; }
        IExpenseRepository Expense {get;}
        IInvoiceRepository Invoice { get; }
        IOrderRepository Order { get; }

        // add more 
        Task<int> Complete();


        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {

        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {

        }

        
    }
}
