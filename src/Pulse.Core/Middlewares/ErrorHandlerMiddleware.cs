using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;

namespace Pulse.Core.Middlewares;

/// <summary>
/// Middleware for handling errors and standardizing error responses
/// </summary>
public class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlerMiddleware> _logger;

    /// <summary>
    /// Initializes a new instance of the ErrorHandlerMiddleware class
    /// </summary>
    /// <param name="next">The next middleware in the pipeline</param>
    /// <param name="logger">Logger instance for error logging</param>
    public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// Processes the HTTP request and handles any errors
    /// </summary>
    /// <param name="context">The HTTP context</param>
    /// <returns>A task representing the asynchronous operation</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);

            if (context.Response is HttpResponse response && response.StatusCode == 404)
            {
                _logger.LogWarning("404 Not Found: {Path}", context.Request.Path);
                await response.WriteAsJsonAsync(new {
                    message = "Not Found"
                });
            }
            else if (context.Response is HttpResponse forbiddenResponse && forbiddenResponse.StatusCode == 403)
            {
                _logger.LogWarning("403 Forbidden: {Path}", context.Request.Path);
                await forbiddenResponse.WriteAsJsonAsync(new {
                    error = "insufficient_permissions",
                    error_description = "Insufficient permissions to access resource",
                    message = "Permission denied"
                });
            }
            else if (context.Response is HttpResponse unauthorizedResponse && unauthorizedResponse.StatusCode == 401)
            {
                _logger.LogWarning("401 Unauthorized: {Path}", context.Request.Path);
                await unauthorizedResponse.WriteAsJsonAsync(
                    new {
                        message = context.Request.Headers.ContainsKey("Authorization")
                                        ? "Bad credentials"
                                        : "Requires authentication"
                    });
            }
        }
        catch (Exception ex)
        {
            await HandleException(context, ex);
        }
    }

    /// <summary>
    /// Handles exceptions that occur during request processing
    /// </summary>
    /// <param name="context">The HTTP context</param>
    /// <param name="ex">The exception that occurred</param>
    /// <returns>A task representing the asynchronous operation</returns>
    private async Task HandleException(HttpContext context, Exception ex)
    {
        _logger.LogError(ex, "Unhandled exception occurred: {Message}", ex.Message);
        
        context.Response.StatusCode = 500;
        await context.Response.WriteAsJsonAsync(new {
            message = "Internal Server Error."
        });
    }
}

/// <summary>
/// Extension methods for registering the ErrorHandlerMiddleware
/// </summary>
public static class ErrorHandlerMiddlewareExtensions
{
    /// <summary>
    /// Adds the error handler middleware to the application pipeline
    /// </summary>
    /// <param name="builder">The application builder</param>
    /// <returns>The application builder for chaining</returns>
    public static IApplicationBuilder UseErrorHandler(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ErrorHandlerMiddleware>();
    }
}
