using DataAccess.Data;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Models.Repositories;

namespace DataAccess.Repositories
{
    public class BaseRepository<T> : IBaseRepositories<T> where T : class
    {
        private readonly ApplicationDbContext Context;
        private DbSet<T> DbSet;
        public BaseRepository(ApplicationDbContext _context)
        {
            Context = _context;
            DbSet = Context.Set<T>();
        }
        public void Alter(T entity)
        {
            Context.Update(entity);
        }

        public void Create(T entity)
        {
            Context.Add(entity);
        }

        public void Delete(T entity)
        {
            Context.Remove(entity);
        }
        public IQueryable<T> Get(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> includeprops = null, bool tracked = true)
        {
            IQueryable<T> query = DbSet;
            if (includeprops != null)
            {
                query = includeprops(query);
            }
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (!tracked)
            {
                query = query.AsNoTracking();
            }
            return query;
        }
        public T? GetOne(Expression<Func<T, bool>>? filter, Func<IQueryable<T>, IIncludableQueryable<T, object>> includeprops = null, bool tracked = true)
        {
            return Get(filter, includeprops).FirstOrDefault();
        }
    }
}
