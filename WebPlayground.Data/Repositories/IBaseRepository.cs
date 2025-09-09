using WebPlayground.Data.Models;

namespace WebPlayground.Data.Repositories
{
    public interface IBaseRepository<T> where T : Entity
    {
        IQueryable<T> GetAll();
        Task<T?> GetByIdAsync(long id);
        Task<T> UpsertAsync(T entity);
    }
}
