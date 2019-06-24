using System;
using System.Diagnostics;
using Samr.ERP.Infrastructure.Data.Contracts;
using Samr.ERP.Infrastructure.Entities;

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
        public UnitOfWork(SamrDbContext dbContext, IRepositoryProvider repositoryProvider)
        {
            DbContext = dbContext;

            repositoryProvider.DbContext = DbContext;
            RepositoryProvider = repositoryProvider;
        }

        // repositories
        public IRepository<User> Users { get { return GetStandardRepo<User>(); } }
        public IRepository<Role> Roles { get { return GetStandardRepo<Role>(); } }
        public IRepository<Employee> Employees { get { return GetStandardRepo<Employee>(); } }


        /// <summary>
        /// Save pending changes to the database
        /// </summary>
        public void Commit()
        {
			
            //System.Diagnostics.Debug.WriteLine("Committed");
			//try
	       // {
		        DbContext.SaveChanges();
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


        protected void CreateDbContext()
        {
            //DbContext = new SamrDbContext();
       
        }

        protected IRepositoryProvider RepositoryProvider { get; set; }

        private IRepository<T> GetStandardRepo<T>() where T : class
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

