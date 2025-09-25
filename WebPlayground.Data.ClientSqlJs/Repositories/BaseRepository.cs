using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebPlayground.Data.Models;
using WebPlayground.Data.Repositories;

namespace WebPlayground.Data.ClientSqlJs.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : Entity
    {
        protected readonly IClientSqlJsService ClientSqlJsService;

        public BaseRepository(IClientSqlJsService clientSqlJsService)
        {
            this.ClientSqlJsService = clientSqlJsService;
        }

        public IQueryable<T> GetAll() => throw new NotImplementedException();

        public async Task<T?> GetByIdAsync(long id) => throw new NotImplementedException();

        public async Task<T> UpsertAsync(T entity)
        {
            throw new NotImplementedException();
            //if (entity.Id == 0)
            //    DbSet.Add(entity);
            //else
            //    DbSet.Update(entity);

            //await Context.SaveChangesAsync();
            //return entity;
        }
    }
}
