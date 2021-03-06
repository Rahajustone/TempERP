﻿using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Samr.ERP.Infrastructure.Data.Contracts;
using Samr.ERP.Infrastructure.Entities;
using Samr.ERP.Infrastructure.Providers;

namespace Samr.ERP.Infrastructure.Data.Concrete
{
    /// <summary>
    /// The "Unit of Work"
    ///     1) decouples the repos from the controllers
    ///     2) decouples the DbContext and EF from the controllers
    ///     3) manages the UoW
    /// </summary>
    /// <remarks>
    /// This class implements the "Unit of Work" pattern in which
    /// the "UoW" serves as a facade for querying and saving to the database.
    /// Querying is delegated to "repositories".
    /// Each repository serves as a container dedicated to a particular
    /// root entity type such as a <see cref="System.Security.Policy.Url"/>.
    /// A repository typically exposes "Get" methods for querying and
    /// will offer add, update, and delete methods if those features are supported.
    /// The repositories rely on their parent UoW to provide the interface to the
    /// data layer (which is the EF DbContext in this example).
    /// </remarks>
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly UserProvider _userProvider;

        public UnitOfWork(SamrDbContext dbContext, IRepositoryProvider repositoryProvider, UserProvider userProvider)
        {
            _userProvider = userProvider;
            DbContext = dbContext;

            repositoryProvider.DbContext = DbContext;
            RepositoryProvider = repositoryProvider;
        }

        // repositories
        public IRepository<User> Users { get => GetStandardRepo<User>(); }
        public IRepository<Role> Roles { get { return GetStandardRepo<Role>(); } }
        public IRepository<Gender> Genders => GetStandardRepo<Gender>();
        public IRepository<Employee> Employees => GetStandardRepo<Employee>();
        public IRepository<EmployeeLog> EmployeeLogs => GetStandardRepo<EmployeeLog>();
        public IRepository<Department> Departments => GetStandardRepo<Department>();
        public IRepository<DepartmentLog> DepartmentLogs => GetStandardRepo<DepartmentLog>();
        public IRepository<EmployeeLockReason> EmployeeLockReasons => GetStandardRepo<EmployeeLockReason>();
        public IRepository<EmployeeLockReasonLog> EmployeeLockReasonLog => GetStandardRepo<EmployeeLockReasonLog>();
        public IRepository<Nationality> Nationalities => GetStandardRepo<Nationality>();
        public IRepository<NationalityLog> NationalityLogs => GetStandardRepo<NationalityLog>();
        public IRepository<Position> Positions => GetStandardRepo<Position>();
        public IRepository<PositionLog> PositionLogs => GetStandardRepo<PositionLog>();
        public IRepository<News> News => GetStandardRepo<News>();
        public IRepository<NewsCategory> NewsCategories => GetStandardRepo<NewsCategory>();
        public IRepository<NewsCategoryLog> NewsCategoryLogs => GetStandardRepo<NewsCategoryLog>();
        public IRepository<EmailSetting> EmailSettings => GetStandardRepo<EmailSetting>();
        public IRepository<EmailMessageHistory> EmailMessageHistories => GetStandardRepo<EmailMessageHistory>();
        public IRepository<UserLockReason> UserLockReasons => GetStandardRepo<UserLockReason>();
        public IRepository<UserLockReasonLog> UserLockReasonLogs => GetStandardRepo<UserLockReasonLog>();
        public IRepository<UsefulLinkCategory> UsefulLinkCategories => GetStandardRepo<UsefulLinkCategory>();
        public IRepository<UsefulLinkCategoryLog> UsefulLinkCategoryLogs => GetStandardRepo<UsefulLinkCategoryLog>();
        public IRepository<UsefulLink> UsefulLinks => GetStandardRepo<UsefulLink>();
        public IRepository<RefreshToken> RefreshTokens => GetStandardRepo<RefreshToken>();
        public IRepository<FileArchiveCategory> FileArchiveCategories => GetStandardRepo<FileArchiveCategory>();
        public IRepository<FileArchiveCategoryLog> FileArchiveCategoryLogs => GetStandardRepo<FileArchiveCategoryLog>();
        public IRepository<FileArchive> FileArchives => GetStandardRepo<FileArchive>();
        public  IRepository<Notification> Notifications => GetStandardRepo<Notification>();
        public  IRepository<ActiveUserToken> ActiveUserTokens => GetStandardRepo<ActiveUserToken>();
        public  IRepository<SMPPSetting> SMPPSettings => GetStandardRepo<SMPPSetting>();
        public IRepository<SMSMessageHistory> SMSMessageHistories => GetStandardRepo<SMSMessageHistory>();
        /// <summary>
        /// Save pending changes to the database
        /// </summary>
        public async Task<int> CommitAsync()
        {
			
            //System.Diagnostics.Debug.WriteLine("Committed");
			//try
	       // {
		        return await DbContext.SaveChangesAsync();
	       // }
		    //catch (Exception ex) // DbEntityValidationException ex)
			//{
			//	Debug.WriteLine(" ");
				//foreach (var error in ex.EntityValidationErrors)
				//{
				//	Debug.WriteLine("====================");
				//	Debug.WriteLine("Entity {0} in state {1} has validation errors:",
				//		error.Entry.Entity.GetType().Name, error.Entry.State);
				//	foreach (var ve in error.ValidationErrors)
				//	{
				//		Debug.WriteLine("\tProperty: {0}, Error: {1}",
				//			ve.PropertyName, ve.ErrorMessage);
				//	}
				//	Debug.WriteLine(" ");
				//}
			    // throw;
			//}
        }
        /// <summary>
        /// Save pending changes to the database
        /// </summary>
        public int Commit()
        {

            //System.Diagnostics.Debug.WriteLine("Committed");
            //try
            // {
            return DbContext.SaveChanges();
            // }
            //catch (Exception ex) // DbEntityValidationException ex)
            //{
            //	Debug.WriteLine(" ");
            //foreach (var error in ex.EntityValidationErrors)
            //{
            //	Debug.WriteLine("====================");
            //	Debug.WriteLine("Entity {0} in state {1} has validation errors:",
            //		error.Entry.Entity.GetType().Name, error.Entry.State);
            //	foreach (var ve in error.ValidationErrors)
            //	{
            //		Debug.WriteLine("\tProperty: {0}, Error: {1}",
            //			ve.PropertyName, ve.ErrorMessage);
            //	}
            //	Debug.WriteLine(" ");
            //}
            // throw;
            //}
        }

        protected IRepositoryProvider RepositoryProvider { get; set; }

        public IRepository<T> GetStandardRepo<T>() where T : class
        {
            return RepositoryProvider.GetRepositoryForEntityType<T>();
        }
        private T GetRepo<T>() where T : class
        {
            return RepositoryProvider.GetRepository<T>();
        }

        private SamrDbContext DbContext { get; set; }

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (DbContext != null)
                {
                    DbContext.Dispose();
                }
            }
        }

        #endregion
   

     
    }
}

