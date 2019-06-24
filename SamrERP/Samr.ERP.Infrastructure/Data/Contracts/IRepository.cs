using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Samr.ERP.Infrastructure.Data.Contracts
{
    public interface IRepository<T> where T : class
    {
        //IQueryable<T> GetAll();
        T GetById(Guid id);
        void Add(T entity);
        Task<T> AddAsync(T entity);
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
        //void Save(T item);
        //IQueryable<T> Find(Func<T, bool> expression);
        //void Attach(T item);        
    }
}