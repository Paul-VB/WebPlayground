using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.Hosting;
using WebPlayground.FunctionApp;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();
FunctionAppServiceRegistrar.RegisterServices(builder.Services, builder.Configuration);


builder.Build().Run();
