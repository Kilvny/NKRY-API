﻿using NKRY_API.DataAccess.EFCore;
using NKRY_API.Domain.Contracts;

namespace NKRY_API.Repositories.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationContext _context;
        private readonly ILogger<UnitOfWork> _logger;
        private IUserRepository _user;
        public IUserRepository User
        {
            get
            {
                if (_user == null)
                {
                    _user = new UserRepository(_context);
                }
                return _user;
            }
        }
        public UnitOfWork(ApplicationContext context, ILogger<UnitOfWork> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<int> Complete()
        {
            var saveResult = await _context.SaveChangesAsync();
            _logger.LogInformation($"Saved {saveResult} to the database successfully!") ;
            return saveResult;
            
        }
        // Flag: Has Dispose already been called?
        bool disposed = false;

        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            _context.Dispose();
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                // Free any other managed objects here.
                //
            }

            // Free any unmanaged objects here.
            //
            disposed = true;
        }

        ~UnitOfWork()
        {
            Dispose(disposing: false);
        }

    }
}