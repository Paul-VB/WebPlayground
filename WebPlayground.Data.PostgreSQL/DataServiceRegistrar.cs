using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WebPlayground.Data.PostgreSQL
{
    public static class DataServiceRegistrar
    {
        public static void RegisterDataServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<MainDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("MainDb"), 
                    o => o.UseVector()));

            // Future: Register repositories here
            // services.AddScoped<IDocumentRepository, DocumentRepository>();
        }
    }
}