using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Pulse.Core.Authorization;
using Pulse.Core.Services;

namespace Pulse.Core.Extensions;

/// <summary>
/// Extension methods for registering Pulse Core services
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds Pulse Core services to the dependency injection container
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddPulseByMirthSystems(this IServiceCollection services)
    {
        // Register business services
        services.AddScoped<IMessageService, MessageService>();

        // Register authorization handlers
        services.AddSingleton<IAuthorizationHandler, RBACHandler>();

        return services;
    }
    
    /// <summary>
    /// Adds Pulse Core authorization policies
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddPulseByMirthSystemsAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy("read:admin-messages", policy =>
            {
                policy.Requirements.Add(new RBACRequirement("read:admin-messages"));
            });
        });
        
        return services;
    }
      /// <summary>
    /// Adds Pulse Core authentication with JWT Bearer
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="configuration">The configuration</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddPulseByMirthSystemsAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var audience = configuration.GetValue<string>("AUTH0_AUDIENCE");
        var domain = configuration.GetValue<string>("AUTH0_DOMAIN");

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = $"https://{domain}/";
                options.Audience = audience;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true
                };
            });

        return services;
    }
}
