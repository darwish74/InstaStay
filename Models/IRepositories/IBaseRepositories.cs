using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Models.Repositories
{
    public interface IBaseRepositories<T> where T : class
    {
        public void Create(T entity);
        public void Alter(T entity);
        public void Delete(T entity);
        public IQueryable<T> Get(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> includeprops = null, bool tracked = true);
        public T? GetOne(Expression<Func<T, bool>>? filter, Func<IQueryable<T>, IIncludableQueryable<T, object>> includeprops = null, bool tracked = true);
        void CreateWithImage(T entity, IFormFile imageFile, string imageFolder, string imageUrlProperty);
        void DeleteWithImage(T entity, string imageFolder, string imageProperty);
    }
}
