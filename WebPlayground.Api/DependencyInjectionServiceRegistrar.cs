using WebPlayground.Core.Services;

namespace WebPlayground.Api
{
    public static class DependencyInjectionServiceRegistrar
    {
        public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddScoped<IOllamaService, OllamaService>();
        }
    }
}
