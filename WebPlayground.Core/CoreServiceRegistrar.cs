using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using WebPlayground.Core.Services;

namespace WebPlayground.Core
{
    public static class CoreServiceRegistrar
    {
        public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IOllamaService, OllamaService>();
        }
    }
}
