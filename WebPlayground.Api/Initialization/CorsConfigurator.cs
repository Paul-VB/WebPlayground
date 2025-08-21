namespace WebPlayground.Api.Initialization
{
    public static class CorsConfigurator
    {
        public static class Policies
        {
            public const string Development = nameof(Development);
            public const string Production = nameof(Production);
        }

        public static void AddCorsPolicies(IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(Policies.Development, policy =>
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod());

                options.AddPolicy(Policies.Production, policy =>
                    policy.WithOrigins(configuration["Cors:ProductionOrigins"] ??
                                     "https://black-plant-047accb0f.1.azurestaticapps.net")
                          .AllowAnyHeader()
                          .AllowAnyMethod());
            });
        }

        public static void UseCorsByEnvironment(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var policy = env.IsDevelopment() ? Policies.Development : Policies.Production;
            app.UseCors(policy);
        }
    }
}