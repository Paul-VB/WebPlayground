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
        
        public IQueryable<T> GetAll() => DbSet;

        public async Task<T?> GetByIdAsync(long id) => await DbSet.FindAsync(id);

        public async Task<T> UpsertAsync(T entity)
        {
            if (entity.Id == 0)
                DbSet.Add(entity);
            else
                DbSet.Update(entity);
            
            await Context.SaveChangesAsync();
            return entity;
        }
    }
}
