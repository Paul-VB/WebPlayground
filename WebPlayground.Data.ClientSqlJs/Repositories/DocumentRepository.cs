using WebPlayground.Data.Models.Ai;
using WebPlayground.Data.Repositories;

namespace WebPlayground.Data.ClientSqlJs.Repositories
{
    public class DocumentRepository : IDocumentRepository
    {
        public DocumentRepository()
        {
            
        }

        public IQueryable<Document> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Document?> GetByIdAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task<Document> UpsertAsync(Document entity)
        {
            throw new NotImplementedException();
        }
    }
}
