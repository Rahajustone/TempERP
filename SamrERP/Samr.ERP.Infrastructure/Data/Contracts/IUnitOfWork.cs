using System;
using Samr.ERP.Infrastructure.Entities;

namespace Samr.ERP.Infrastructure.Data.Contracts
{
    /// <summary>
    /// Interface for the "Unit of Work"
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        // Save pending changes to the data store.
        void Commit();
        // Repositories

        IRepository<User> Users { get; }
        IRepository<Role> Roles { get; }
        IRepository<Employee> Employees { get; }

    }
}
