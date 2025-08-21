using Microsoft.AspNetCore.Diagnostics;
using WebPlayground.Core.Exceptions;

namespace WebPlayground.Api.Initialization
{
    public static class CustomExceptionHandler
    {
        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder app)
        {
            return app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "application/json";

                    var exceptionFeature = context.Features.Get<IExceptionHandlerFeature>();
                    var exception = exceptionFeature?.Error;

                    var message = exception is LoggableException le ? le.Message : "An error occurred";
                    await context.Response.WriteAsJsonAsync(new { error = message });
                });
            });
        }
    }
}