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
using Microsoft.AspNetCore.Http;

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
        public void CreateWithImage(T entity, IFormFile imageFile, string imageFolder, string imageUrlProperty)
        {
            if (imageFile != null && imageFile.Length > 0)
            {

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot\\images\\{imageFolder}");

                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                var filePath = Path.Combine(directoryPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    imageFile.CopyTo(stream);
                }

                var property = typeof(T).GetProperty(imageUrlProperty);
                if (property != null)
                {
                    property.SetValue(entity, fileName);
                }
            }

            DbSet.Add(entity);
            Context.SaveChanges();
        }
        public void DeleteWithImage(T entity, string imageFolder, string imageProperty)
        {
            var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot\\images\\{imageFolder}\\{imageProperty}");
            if (File.Exists(oldFilePath))
            {
                File.Delete(oldFilePath);
            }
            DbSet.Remove(entity);

        }
    }
}
