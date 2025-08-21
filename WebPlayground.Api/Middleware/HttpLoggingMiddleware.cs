using WebPlayground.Core.Exceptions;

namespace WebPlayground.Api.Middleware
{
    public class HttpLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<HttpLoggingMiddleware> _logger;

        public HttpLoggingMiddleware(RequestDelegate next, ILogger<HttpLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
                if (context.Response.StatusCode >= 400)
                    LogResponse (context);
            }
            catch (LoggableException ex) when (ex.IsLogged)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception for {Method} {Path}",
                    context.Request.Method,
                    context.Request.Path);
                throw;
            }
        }

        private void LogResponse (HttpContext context)
        {
            var statusCode = context.Response.StatusCode;
            var method = context.Request.Method;
            var path = context.Request.Path;

            if (statusCode >= 500)
            {
                _logger.LogError("Request {Method} {Path} returned {StatusCode}",
                    method, path, statusCode);
            }
            else if (statusCode >= 400)
            {
                _logger.LogWarning("Request {Method} {Path} returned {StatusCode}",
                    method, path, statusCode);
            }
        }
    }
}
