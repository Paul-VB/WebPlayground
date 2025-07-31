using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using WebPlayground.Core;

namespace WebPlayground.FunctionApp
{
    public static class FunctionAppServiceRegistrar
    {
        public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
        {
            // Register core services
            CoreServiceRegistrar.RegisterServices(services, configuration);
            // Add function app specific services here if needed
        }
    }
}
