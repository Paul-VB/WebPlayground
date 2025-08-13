using WebPlayground.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        ApiServiceRegistrar.RegisterServices(builder.Services, builder.Configuration);
        ConfigureCors(builder.Services);

        var app = builder.Build();
        ConfigureMiddleware(app);

        app.Run();
    }

    private static void ConfigureCors(IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("DevelopmentCorsPolicy",
                policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });

            options.AddPolicy("ProductionCorsPolicy",
                policy =>
                {
                    policy.WithOrigins(
                            "https://black-plant-047accb0f.1.azurestaticapps.net"
                        )
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
        });
    }

    private static void ConfigureMiddleware(WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseCors("DevelopmentCorsPolicy");
        }
        else
        {
            app.UseCors("ProductionCorsPolicy");
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
    }
}