using Microsoft.AspNetCore.Diagnostics;
using WebPlayground.Core.Exceptions;

namespace WebPlayground.Api.Initialization
{
    // TODO: Implement proper exception categorization:
    // Category A - Unexpected/Bugs (NullRef, Timeout) → Log as Error, return 500 with generic message
    // Category B - Expected/Recoverable (OllamaOffline) → Log as Warning, return 503/400 with helpful message  
    // Category C - Expected/NotRecoverable (QueryId not found) → Log as Debug, handle locally, don't throw
    // Create specific exception types: OllamaOfflineException : LoggableException, etc.
    // Update middleware to return appropriate status codes based on exception type
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