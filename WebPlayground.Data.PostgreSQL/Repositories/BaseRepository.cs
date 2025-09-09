using Microsoft.EntityFrameworkCore;
using WebPlayground.Data.Models;
using WebPlayground.Data.Repositories;

namespace WebPlayground.Data.PostgreSQL.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : Entity
    {
        protected readonly MainDbContext Context;
        protected readonly DbSet<T> DbSet;
        public BaseRepository(MainDbContext context)
        {
            Context = context;
            DbSet = context.Set<T>();
        }
        public IEnumerable<T> GetAll() => DbSet;

        public T? GetById(long id) => DbSet.Where(e => e.Id == id).FirstOrDefault();

        public T Upsert(T entity)
        {
            if (entity.Id == 0)
                DbSet.Add(entity);
            else
                DbSet.Update(entity);
            Context.SaveChanges();
            return entity;
        }
    }
}
