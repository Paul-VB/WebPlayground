using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using WebPlayground.Infrastructure.WebSocket.Services;

namespace WebPlayground.Infrastructure
{
    public static class InfrastructureServiceRegistrar
    {
        public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
        {
            // WebSocket services
            services.AddScoped<IWebSocketService, WebSocketService>();
        }
    }
}