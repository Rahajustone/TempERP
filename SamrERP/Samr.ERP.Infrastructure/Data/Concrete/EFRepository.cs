using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Samr.ERP.Infrastructure.Data.Contracts;
using Samr.ERP.Infrastructure.Interfaces;

namespace Samr.ERP.Infrastructure.Data.Concrete
{
    /// <summary>
    /// The EF-dependent, generic repository for data access
    /// </summary>
    /// <typeparam name="T">Type of entity for this Repository.</typeparam>
    public class EFRepository<T> : IRepository<T> where T : class
    {
        public EFRepository(DbContext dbContext)
        {
            if (dbContext == null)
                throw new ArgumentNullException("dbContext");
            DbContext = dbContext;
            DbSet = DbContext.Set<T>();
        }

        protected DbContext DbContext { get; set; }

        protected DbSet<T> DbSet { get; set; }

        //public virtual IQueryable<T> GetAll()
        //{
        //    return DbSet;
        //}

        public virtual T GetById(Guid id)
        {
            
            //return DbSet.FirstOrDefault(_=> ((_ as IBaseObject)).Id == id);
            return DbSet.Find(id);
        }

        public virtual void Add(T entity)
        {
	        if (entity is IChangeable)
	        {
				((IChangeable)entity).Created = DateTime.Now.AddHours(2);
				((IChangeable)entity).Updated = DateTime.Now.AddHours(2);
	        }
            EntityEntry dbEntityEntry = DbContext.Entry(entity);
            if (dbEntityEntry.State != EntityState.Detached)
            {
                dbEntityEntry.State = EntityState.Added;
            }
            else
            {
                DbSet.Add(entity);
            }
        }
		public virtual void AddList(List<T> entity)
		{
			EntityEntry dbEntityEntry;
			if (entity != null && entity.Count > 0)
			{
				var temp = entity.FirstOrDefault();
				if (temp is IChangeable)
				{
					foreach (var ent in entity)
					{
						((IChangeable)ent).Created = DateTime.Now.AddHours(2);
						((IChangeable)ent).Updated = DateTime.Now.AddHours(2);
					}
				}
				dbEntityEntry = DbContext.Entry(temp);
			}
			else
			{
				return;
			}
	       
            
            if (dbEntityEntry.State != EntityState.Detached)
            {
                dbEntityEntry.State = EntityState.Added;
            }
            else
            {
                DbSet.AddRange(entity);
            }
        }

        public virtual void Update(T entity)
        {
            EntityEntry dbEntityEntry = DbContext.Entry(entity);
            if (dbEntityEntry.State == EntityState.Detached)
            {
                DbSet.Attach(entity);
            }
            dbEntityEntry.State = EntityState.Modified;

            if (entity is IChangeable)
            {
                ((IChangeable) entity).Updated = DateTime.Now;//GetDateTime();
                DbContext.Entry((IChangeable)entity).Property(x => x.Created).IsModified = false;
            }
            
        }

        //public DateTime GetDateTime()
        //{
        //    return DbContext.Database.SqlQuery<DateTime>("select GETDATE()").Single();
        //}

        public virtual void Delete(T entity)
        {
            if (entity is IDeletable)
            {
                (entity as IDeletable).IsDeleted = true;
                Update(entity);
            }
            else
            {
                EntityEntry dbEntityEntry = DbContext.Entry(entity);
                if (dbEntityEntry.State != EntityState.Deleted)
                {
                    dbEntityEntry.State = EntityState.Deleted;
                }
                else
                {
                    DbSet.Attach(entity);
                    DbSet.Remove(entity);
                }
            }
        }

        public virtual void Reload(T entity)
        {
            DbContext.Entry<T>(entity).Reload();
        }

        public virtual void Delete(Guid id)
        {
            var entity = GetById(id);
            if (entity == null) return; // not found; assume already deleted.
            Delete(entity);
        }

        public DbSet<T> GetDbSet()
        {
            return DbSet;
        }

        
        public void Refresh(IEnumerable<T> list)
        {
            throw new NotImplementedException();
        }

        IQueryable<T> IRepository<T>.All()
        {
            
            //if (typeof(T).GetInterfaces().Contains(typeof(IDeletable)))
            //{
            //    return DbSet.AsQueryable<T>().Where(_ => (( IDeletable)_).IsDeleted == false).Cast<T>();
            //}
            return DbSet .AsQueryable<T>();
            
        }
    }
}
