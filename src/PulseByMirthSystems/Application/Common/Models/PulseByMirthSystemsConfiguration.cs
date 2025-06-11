namespace PulseByMirthSystems.Application.Common.Models;

/// <summary>
/// Configuration options for PulseByMirthSystems library
/// </summary>
public class PulseByMirthSystemsConfiguration
{
    /// <summary>
    /// PostgreSQL connection string
    /// </summary>
    public string ConnectionString { get; set; } = string.Empty;

    /// <summary>
    /// Azure Maps API key for geocoding services
    /// </summary>
    public string? AzureMapsKey { get; set; }

    /// <summary>
    /// Whether to automatically run database migrations on startup
    /// </summary>
    public bool AutoMigrate { get; set; } = true;

    /// <summary>
    /// Whether to seed initial data (admin user, lookup tables)
    /// </summary>
    public bool SeedData { get; set; } = true;

    /// <summary>
    /// Default admin user email for seeding
    /// </summary>
    public string AdminEmail { get; set; } = "admin@pulse.local";

    /// <summary>
    /// Default admin user password for seeding
    /// </summary>
    public string AdminPassword { get; set; } = "Admin123!";

    /// <summary>
    /// JWT signing key for tokens
    /// </summary>
    public string? JwtSigningKey { get; set; }

    /// <summary>
    /// JWT issuer
    /// </summary>
    public string JwtIssuer { get; set; } = "PulseByMirthSystems";

    /// <summary>
    /// JWT audience
    /// </summary>
    public string JwtAudience { get; set; } = "PulseByMirthSystems";

    /// <summary>
    /// File storage base path for local storage
    /// </summary>
    public string FileStoragePath { get; set; } = "App_Data/Files";

    /// <summary>
    /// Maximum file upload size in bytes
    /// </summary>
    public long MaxFileUploadSize { get; set; } = 10 * 1024 * 1024; // 10MB

    /// <summary>
    /// SignalR connection timeout in seconds
    /// </summary>
    public int SignalRTimeoutSeconds { get; set; } = 30;

    /// <summary>
    /// Whether to enable Serilog PostgreSQL sink
    /// </summary>
    public bool EnableDatabaseLogging { get; set; } = true;

    /// <summary>
    /// Minimum log level for Serilog
    /// </summary>
    public string LogLevel { get; set; } = "Information";
}
