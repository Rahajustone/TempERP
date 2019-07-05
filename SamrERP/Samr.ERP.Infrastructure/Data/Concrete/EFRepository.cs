using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Samr.ERP.Infrastructure.Data.Contracts;
using Samr.ERP.Infrastructure.Data.Helpers;
using Samr.ERP.Infrastructure.Entities.BaseObjects;
using Samr.ERP.Infrastructure.Interfaces;
using Samr.ERP.Infrastructure.Providers;

namespace Samr.ERP.Infrastructure.Data.Concrete
{
    /// <summary>
    /// The EF-dependent, generic repository for data access
    /// </summary>
    /// <typeparam name="T">Type of entity for this Repository.</typeparam>
    public class EFRepository<T> : IRepository<T> where T : class
    {
        private readonly UserProvider _userProvider;

        public EFRepository(DbContext dbContext, UserProvider userProvider)
        {
            if (dbContext == null)
                throw new ArgumentNullException(nameof(dbContext));
            _userProvider = userProvider;
            DbContext = dbContext;
            DbSet = DbContext.Set<T>();
        }

        protected DbContext DbContext { get; set; }

        protected DbSet<T> DbSet { get; set; }

        public virtual T GetById(Guid id)
        {
            return DbSet.Find(id);
        }
     
        public async Task<T> GetByIdAsync(Guid id)
        {
            return await DbSet.FindAsync(id);

        }

        public void Add(T entity)
        {
            if (entity is IChangeable changeable)
            {
                changeable.Created = DateTime.Now;
                changeable.Updated = DateTime.Now;
            }

            if (entity is ICreatable creatable)
                creatable.CreatedAt = DateTime.Now;

            if (entity is ICreatableByUser creatableByUser)
                creatableByUser.CreatedUserId = _userProvider.CurrentUser.Id;
            
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
						((IChangeable)ent).Created = DateTime.Now;
						((IChangeable)ent).Updated = DateTime.Now;
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
            if (entity is ICreatableByUser creatableByUser)
                DbContext.Entry(creatableByUser).Property(x => x.CreatedUserId).IsModified = false;
            if (entity is IChangeable changeable)
            {
                changeable.Updated = DateTime.Now;//GetDateTime();
                DbContext.Entry(changeable).Property(x => x.Created).IsModified = false;
            }
            
        }
        
        //public DateTime GetDateTime()
        //{
        //    return DbContext.Database.SqlQuery<DateTime>("select GETDATE()").Single();
        //}
        
        public virtual void Delete(T entity)
        {
            if (entity is IDeletable deletable)
            {
                deletable.IsDeleted = true;
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

        public virtual void DeActivate(T entity)
        {
            if (entity is IActivable activable)
            {
                activable.IsActive = true;
                Update(entity);
            }
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await DbSet.AnyAsync(p => ((IBaseObject) p).Id == id);
        }

        public bool Exists(Guid id)
        {
            return Exists(id);
        }

        public bool Any(Expression<Func<T, bool>> predicate)
        {
            return DbSet.Any(predicate);
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            return await DbSet.AnyAsync(predicate);
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
            return DbSet.AsQueryable<T>();
            
        }

        public IEnumerable<T> GetAll()
        {
            return DbSet;
        }
     



    }
}
