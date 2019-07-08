﻿using System;
using System.Threading.Tasks;
using Samr.ERP.Infrastructure.Entities;

namespace Samr.ERP.Infrastructure.Data.Contracts
{
    /// <summary>
    /// Interface for the "Unit of Work"
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        // Save pending changes to the data store.
        Task<int> CommitAsync();
        // Repositories

        IRepository<User> Users { get; }
        IRepository<Role> Roles { get; }
        IRepository<Gender> Genders { get; }
        IRepository<Employee> Employees { get; }
        IRepository<Department> Departments { get; }
        IRepository<EmployeeLockReason> EmployeeLockReasons { get; }
        IRepository<Nationality> Nationalities { get; }
        IRepository<Position> Positions { get;  }
        IRepository<News> News { get; }
        IRepository<NewsCategory> NewsCategories { get; }
        IRepository<EmailSetting> EmailSettings{ get; }
        IRepository<EmailMessageHistory> EmailMessageHistories { get; }
        IRepository<UserLockReason> UserLockReasons { get; }


    }
}
