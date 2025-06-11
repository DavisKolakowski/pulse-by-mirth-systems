// TODO: This will be moved to a separate web extensions package
// since this is a class library project and doesn't have access to IApplicationBuilder

/*
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PulseByMirthSystems.Infrastructure.Data.Context;

namespace PulseByMirthSystems.Extensions;

/// <summary>
/// Application builder extensions for PulseByMirthSystems endpoints and middleware
/// </summary>
public static class ApplicationBuilderExtensions
{
    /// <summary>
    /// Adds PulseByMirthSystems endpoints and middleware to the application pipeline
    /// </summary>
    /// <param name="app">The application builder</param>
    /// <returns>The application builder for chaining</returns>
    public static IApplicationBuilder UsePulseByMirthSystems(this IApplicationBuilder app)
    {
        // Get configuration and services
        var config = app.ApplicationServices.GetRequiredService<PulseByMirthSystemsConfiguration>();
        var logger = app.ApplicationServices.GetRequiredService<ILogger<PulseByMirthSystemsConfiguration>>();

        // Auto-migrate database if configured
        if (config.AutoMigrate)
        {
            logger.LogInformation("Auto-migrating database...");
            using var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            context.Database.Migrate();
        }

        // Seed initial data if configured
        if (config.SeedData)
        {
            logger.LogInformation("Seeding initial data...");
            SeedInitialData(app.ApplicationServices, config, logger);
        }

        return app;
    }

    /// <summary>
    /// Adds PulseByMirthSystems REST API endpoints to the application
    /// </summary>
    /// <param name="app">The application builder</param>
    /// <returns>The application builder for chaining</returns>
    public static IApplicationBuilder AddPulseByMirthSystemsEndpoints(this IApplicationBuilder app)
    {
        // Map SignalR hubs
        // TODO: Add SignalR hub mappings
        // app.MapHub<ActivityThreadHub>("/hubs/activity");
        // app.MapHub<NotificationHub>("/hubs/notifications");

        // TODO: Map API controllers
        // This would typically be done with MapControllers() in the main app
        // The controllers are in the Presentation layer

        return app;
    }

    private static void SeedInitialData(IServiceProvider serviceProvider, PulseByMirthSystemsConfiguration config, ILogger logger)
    {
        using var scope = serviceProvider.CreateScope();
        var services = scope.ServiceProvider;

        try
        {
            // TODO: Implement seeding logic
            // var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            // var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();
            // var context = services.GetRequiredService<ApplicationDbContext>();

            // Seed lookup tables (venue categories, special categories, etc.)
            // SeedLookupTables(context);

            // Seed admin user
            // SeedAdminUser(userManager, roleManager, config);

            logger.LogInformation("Initial data seeding completed successfully");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding initial data");
            throw;
        }
    }
}
*/
