using WebPlayground.Data.Models;

namespace WebPlayground.Data.Repositories
{
    public interface IBaseRepository<T> where T : Entity
    {
        public IEnumerable<T> GetAll();
        public T? GetById(long id);
        public T Upsert(T entity);
    }
}
