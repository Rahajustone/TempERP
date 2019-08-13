using System;
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
        IRepository<DepartmentLog> DepartmentLogs { get; }
        IRepository<EmployeeLockReason> EmployeeLockReasons { get; }
        IRepository<Nationality> Nationalities { get; }
        IRepository<NationalityLog> NationalityLogs { get; }
        IRepository<Position> Positions { get; }
        IRepository<PositionLog> PositionLogs { get; }
        IRepository<News> News { get; }
        IRepository<NewsCategory> NewsCategories { get; }
        IRepository<NewsCategoryLog> NewsCategoryLogs { get; }
        IRepository<EmailSetting> EmailSettings { get; }
        IRepository<EmailMessageHistory> EmailMessageHistories { get; }
        IRepository<UserLockReason> UserLockReasons { get; }
        IRepository<UserLockReasonLog> UserLockReasonLogs { get; }
        IRepository<UsefulLinkCategory> UsefulLinkCategories { get; }
        IRepository<UsefulLink> UsefulLinks { get; }
        IRepository<RefreshToken> RefreshTokens { get; }
        IRepository<FileArchiveCategory> FileArchiveCategories { get; }
        IRepository<FileArchiveCategoryLog> FileArchiveCategoryLogs { get; }
        IRepository<FileArchive> FileArchives { get; }
        IRepository<Notification> Notifications { get; }
        IRepository<ActiveUserToken> ActiveUserTokens { get; }
        IRepository<EmployeeLockReasonLog> EmployeeLockReasonLog { get; }
        IRepository<UsefulLinkCategoryLog> UsefulLinkCategoryLogs { get; }
        IRepository<T> GetStandardRepo<T>() where T : class;
    }
}
