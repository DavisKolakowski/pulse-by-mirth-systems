using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NodaTime;
using PulseByMirthSystems.Application.Common.Models;
using PulseByMirthSystems.Domain.Entities;
using PulseByMirthSystems.Infrastructure.Data.Context;
using Serilog;
using Serilog.Events;

namespace PulseByMirthSystems.Extensions;

/// <summary>
/// Service collection extensions for registering PulseByMirthSystems services
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds PulseByMirthSystems services to the service collection
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="configuration">Configuration action</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddPulseByMirthSystems(
        this IServiceCollection services,
        Action<PulseByMirthSystemsConfiguration> configuration)
    {
        var config = new PulseByMirthSystemsConfiguration();
        configuration(config);

        // Validate required configuration
        if (string.IsNullOrWhiteSpace(config.ConnectionString))
        {
            throw new ArgumentException("ConnectionString is required for PulseByMirthSystems");
        }

        // Register configuration
        services.AddSingleton(config);

        // Configure logging with Serilog
        ConfigureSerilog(services, config);

        // Register NodaTime IClock
        services.AddSingleton<IClock>(SystemClock.Instance);
        ConfigureDatabase(services, config);

        // Configure ASP.NET Core Identity
        ConfigureIdentity(services, config);

        // Register application services
        RegisterApplicationServices(services, config);
        RegisterInfrastructureServices(services, config);

        // Configure SignalR
        ConfigureSignalR(services, config);

        return services;
    }

    private static void ConfigureSerilog(IServiceCollection services, PulseByMirthSystemsConfiguration config)
    {
        var loggerConfiguration = new LoggerConfiguration()
            .MinimumLevel.Is(Enum.Parse<LogEventLevel>(config.LogLevel))
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("System", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {NewLine}{Exception}");

        if (config.EnableDatabaseLogging)
        {
            loggerConfiguration.WriteTo.PostgreSQL(
                connectionString: config.ConnectionString,
                tableName: "logs",
                needAutoCreateTable: true,
                restrictedToMinimumLevel: LogEventLevel.Information);
        }

        Log.Logger = loggerConfiguration.CreateLogger();
        services.AddLogging(builder => builder.AddSerilog(dispose: true));
    }

    private static void ConfigureDatabase(IServiceCollection services, PulseByMirthSystemsConfiguration config)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(config.ConnectionString, npgsqlOptions =>
            {
                npgsqlOptions.UseNodaTime();
                npgsqlOptions.UseNetTopologySuite();
            })
            .UseSnakeCaseNamingConvention();

            // Enable sensitive data logging in development
            #if DEBUG
            options.EnableSensitiveDataLogging();
            options.EnableDetailedErrors();
            #endif
        });
    }

    private static void ConfigureIdentity(IServiceCollection services, PulseByMirthSystemsConfiguration config)
    {
        services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
        {
            // Password settings
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequiredLength = 8;

            // Lockout settings
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;

            // User settings
            options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            options.User.RequireUniqueEmail = true;

            // Sign in settings
            options.SignIn.RequireConfirmedEmail = false;
            options.SignIn.RequireConfirmedPhoneNumber = false;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

        // Cookie configuration will be done in the consuming application
    }

    private static void RegisterApplicationServices(IServiceCollection services, PulseByMirthSystemsConfiguration config)
    {
        // Register application services
        // TODO: Add more services as they are implemented
        // services.AddScoped<INotificationService, NotificationService>();
        // services.AddScoped<IPostService, PostService>();
        // services.AddScoped<IActivityThreadService, ActivityThreadService>();
    }
    
    private static void RegisterInfrastructureServices(IServiceCollection services, PulseByMirthSystemsConfiguration config)
    {
        // Register repositories
        RegisterRepositories(services);
        
        // TODO: Register other infrastructure services here
        // services.AddScoped<IFileStorageService, LocalFileStorageService>();
        // services.AddScoped<INotificationService, SignalRNotificationService>();
        
        // Azure Maps services (if key provided)
        if (!string.IsNullOrWhiteSpace(config.AzureMapsKey))
        {
            // TODO: Register Azure Maps services
            // services.AddScoped<IGeocodingService, AzureMapsGeocodingService>();
        }
    }    
    
    private static void RegisterRepositories(IServiceCollection services)
    {
        // Register specific repositories with their interfaces
        
        // Note: BaseRepository<,> is abstract and should not be registered directly.
        // Only concrete implementations should be registered with the DI container.
    }

    private static void ConfigureSignalR(IServiceCollection services, PulseByMirthSystemsConfiguration config)
    {
        services.AddSignalR(options =>
        {
            options.ClientTimeoutInterval = TimeSpan.FromSeconds(60); // Default 60 seconds
            options.HandshakeTimeout = TimeSpan.FromSeconds(15);
            options.KeepAliveInterval = TimeSpan.FromSeconds(15);
        });
    }
}