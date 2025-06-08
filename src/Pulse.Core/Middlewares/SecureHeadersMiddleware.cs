using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;

namespace Pulse.Core.Middlewares;

/// <summary>
/// Middleware for adding security headers to HTTP responses
/// </summary>
public class SecureHeadersMiddleware
{
    private readonly RequestDelegate _next;

    /// <summary>
    /// Initializes a new instance of the SecureHeadersMiddleware class
    /// </summary>
    /// <param name="next">The next middleware in the pipeline</param>
    public SecureHeadersMiddleware(RequestDelegate next)
    {
        _next = next;
    }    /// <summary>
    /// Processes the HTTP request and adds security headers
    /// </summary>
    /// <param name="context">The HTTP context</param>
    /// <returns>A task representing the asynchronous operation</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        // Add security headers
        context.Response.Headers["X-XSS-Protection"] = "0";
        context.Response.Headers["Strict-Transport-Security"] = "max-age=31536000; includeSubDomains";
        context.Response.Headers["X-Frame-Options"] = "deny";
        context.Response.Headers["X-Content-Type-Options"] = "nosniff";
        context.Response.Headers["Content-Security-Policy"] = "default-src 'self'; frame-ancestors 'none';";
        context.Response.Headers["Cache-Control"] = "no-cache, no-store, max-age=0, must-revalidate";
        context.Response.Headers["Pragma"] = "no-cache";

        await _next(context);
    }
}

/// <summary>
/// Extension methods for registering the SecureHeadersMiddleware
/// </summary>
public static class SecureHeadersMiddlewareExtensions
{
    /// <summary>
    /// Adds the secure headers middleware to the application pipeline
    /// </summary>
    /// <param name="builder">The application builder</param>
    /// <returns>The application builder for chaining</returns>
    public static IApplicationBuilder UseSecureHeaders(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<SecureHeadersMiddleware>();
    }
}
