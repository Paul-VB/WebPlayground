using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebPlayground.Core.Services;

namespace WebPlayground.FunctionApp
{
    public static class DependencyInjectionServiceRegistrar
    {
        public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IOllamaService, OllamaService>();
        }
    }
}
