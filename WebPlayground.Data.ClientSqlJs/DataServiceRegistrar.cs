using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebPlayground.Data.ClientSqlJs.Repositories;
using WebPlayground.Data.Repositories;

namespace WebPlayground.Data.ClientSqlJs
{
    public static class DataServiceRegistrar
    {
        public static void RegisterDataServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IClientSqlJsService, ClientSqlJsService>();
            services.AddScoped<IDocumentRepository, DocumentRepository>();
        }
    }
}