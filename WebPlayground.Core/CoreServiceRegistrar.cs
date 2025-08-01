using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using WebPlayground.Core.Services;
using WebPlayground.Core.Helpers;

namespace WebPlayground.Core
{
    public static class CoreServiceRegistrar
    {
        public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient<IHttpClientWrapper, HttpClientWrapper>();
            services.AddScoped<IOllamaService, OllamaService>();
        }
    }
}
