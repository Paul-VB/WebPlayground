using Microsoft.AspNetCore.WebSockets;
using WebPlayground.Core;
using WebPlayground.Infrastructure;

namespace WebPlayground.Api.Initialization
{
    public static class ApiServiceRegistrar
    {
        public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddWebSockets(options => { });
            services.AddControllers();
            
            // Session configuration
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.AddHttpContextAccessor();
            
            ConfigureSwagger(services);
            
            CoreServiceRegistrar.RegisterServices(services, configuration);
            InfrastructureServiceRegistrar.RegisterServices(services, configuration);
            //Data.PostgreSQL.DataServiceRegistrar.RegisterDataServices(services, configuration);
            Data.ClientSqlJs.DataServiceRegistrar.RegisterDataServices(services, configuration);
        }

        private static void ConfigureSwagger(IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }
    }
}
