using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Hosting;
using WebPlayground.FunctionApp;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();
DependencyInjectionServiceRegistrar.RegisterServices(builder.Services, builder.Configuration);

// Application Insights isn't enabled by default. See https://aka.ms/AAt8mw4.
// builder.Services
//     .AddApplicationInsightsTelemetryWorkerService()
//     .ConfigureFunctionsApplicationInsights();

builder.Build().Run();
