using WebPlayground.Api.Initialization;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        ApiServiceRegistrar.RegisterServices(builder.Services, builder.Configuration);
        CorsConfigurator.AddCorsPolicies(builder.Services, builder.Configuration);

        var app = builder.Build();
        ConfigureMiddleware(app);

        app.Run();
    }

    private static void ConfigureMiddleware(WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseCors(CorsConfigurator.Policies.Development);
        }
        else
        {
            app.UseCors(CorsConfigurator.Policies.Production);
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
    }
}