using WebPlayground.Core;

namespace WebPlayground.Api
{
    public static class ApiServiceRegistrar
    {
        public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            CoreServiceRegistrar.RegisterServices(services, configuration);
        }
    }
}
