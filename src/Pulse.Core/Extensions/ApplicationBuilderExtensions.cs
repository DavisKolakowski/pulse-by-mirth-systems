using Microsoft.AspNetCore.Builder;
using Pulse.Core.Middlewares;

namespace Pulse.Core.Extensions;

/// <summary>
/// Extension methods for configuring the application pipeline
/// </summary>
public static class ApplicationBuilderExtensions
{
    /// <summary>
    /// Configures the Pulse Core middleware pipeline
    /// </summary>
    /// <param name="app">The application builder</param>
    /// <returns>The application builder for chaining</returns>
    public static IApplicationBuilder UsePulseByMirthSystems(this IApplicationBuilder app)
    {
        app.UseErrorHandler();
        app.UseSecureHeaders();
        
        return app;
    }
}
