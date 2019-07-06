using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Samr.ERP.Infrastructure.Data.Concrete;

namespace Samr.ERP.Infrastructure.Data.Contracts
{
    public interface IRepository<T> where T : class
    {
        //IQueryable<T> GetAll();
        T GetById(Guid id);
        Task<T> GetByIdAsync(Guid id);
        void Add(T entity);
        void AddList(List<T> entity);
        void Update(T entity);
        void Delete(T entity);
        void Reload(T entity);
        void Delete(Guid id);
        DbSet<T> GetDbSet();
        void Refresh(IEnumerable<T> list);
        //IUnitOfWork UnitOfWork { get; set; }
        IQueryable<T> All();
        IEnumerable<T> GetAll();
        void DeActivate(T entity);

        Task<bool> ExistsAsync(Guid id);
        bool Exists(Guid id);
        bool Any(Expression<Func<T, bool>> predicate);
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);
    }
}