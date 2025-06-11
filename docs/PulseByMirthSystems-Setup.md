# PulseByMirthSystems Setup & Configuration Guide

## Quick Start

### 1. Installation

Add the PulseByMirthSystems library to your ASP.NET Core project:

```xml
<PackageReference Include="PulseByMirthSystems" Version="1.0.0" />
```

### 2. Basic Configuration

In your `Program.cs` or `Startup.cs`:

```csharp
using PulseByMirthSystems.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add PulseByMirthSystems services
builder.Services.AddPulseByMirthSystems(config =>
{
    config.ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    config.AzureMapsKey = builder.Configuration["AzureMaps:Key"];
    config.AutoMigrate = true;
    config.SeedData = true;
});

var app = builder.Build();

// Add PulseByMirthSystems endpoints and SignalR hubs
app.AddPulseByMirthSystemsEndpoints();

app.Run();
```

### 3. Configuration Settings

Add to your `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=pulse;Username=pulse_user;Password=your_password"
  },
  "AzureMaps": {
    "Key": "your_azure_maps_key"
  },
  "PulseByMirthSystems": {
    "AutoMigrate": true,
    "SeedData": true,
    "Identity": {
      "RequireConfirmedEmail": false,
      "RequireDigit": false,
      "RequiredLength": 6,
      "LockoutDefaultLockoutTimeSpan": "00:15:00",
      "MaxFailedAccessAttempts": 5
    },
    "Authorization": {
      "EnableClaimsCaching": true,
      "ClaimsCacheExpiration": "01:00:00"
    },
    "SignalR": {
      "EnableDetailedErrors": true,
      "ClientTimeoutInterval": "00:01:00",
      "KeepAliveInterval": "00:00:15"
    },
    "Logging": {
      "EnablePostgreSQLSink": true,
      "MinimumLevel": "Information"
    },
    "FileStorage": {
      "Provider": "Local",
      "LocalPath": "./uploads",
      "MaxFileSize": 10485760,
      "AllowedExtensions": [".jpg", ".jpeg", ".png", ".gif", ".mp4", ".mov"]
    }
  }
}
```

## Detailed Configuration

### Database Configuration

#### PostgreSQL Setup

1. **Install PostgreSQL** (version 13+ recommended)

2. **Create Database and User**:
```sql
CREATE DATABASE pulse;
CREATE USER pulse_user WITH ENCRYPTED PASSWORD 'your_secure_password';
GRANT ALL PRIVILEGES ON DATABASE pulse TO pulse_user;

-- Enable required extensions
\c pulse
CREATE EXTENSION IF NOT EXISTS "uuid-ossp";
CREATE EXTENSION IF NOT EXISTS "postgis";
CREATE EXTENSION IF NOT EXISTS "fuzzystrmatch";
```

3. **Connection String Configuration**:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=pulse;Username=pulse_user;Password=your_secure_password;Include Error Detail=true"
  }
}
```

#### Entity Framework Migrations

The library automatically applies migrations on startup when `AutoMigrate` is enabled. To manually manage migrations:

```bash
# From your application project
dotnet ef migrations add InitialCreate --project ../PulseByMirthSystems
dotnet ef database update --project ../PulseByMirthSystems
```

### Identity Configuration

#### User Management Settings

```csharp
services.AddPulseByMirthSystems(config =>
{
    config.Identity = new IdentityConfiguration
    {
        RequireConfirmedEmail = false,
        RequireDigit = false,
        RequireLowercase = false,
        RequireUppercase = false,
        RequireNonAlphanumeric = false,
        RequiredLength = 6,
        RequiredUniqueChars = 1,
        
        // Lockout settings
        LockoutDefaultLockoutTimeSpan = TimeSpan.FromMinutes(15),
        LockoutMaxFailedAccessAttempts = 5,
        AllowedForNewUsers = true,
        
        // User settings
        AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+",
        RequireUniqueEmail = true
    };
});
```

#### Cookie Configuration

```csharp
config.Identity.CookieConfiguration = new CookieConfiguration
{
    LoginPath = "/Account/Login",
    LogoutPath = "/Account/Logout",
    AccessDeniedPath = "/Account/AccessDenied",
    ExpireTimeSpan = TimeSpan.FromDays(30),
    SlidingExpiration = true,
    HttpOnly = true,
    SecurePolicy = CookieSecurePolicy.Always
};
```

### Authorization Configuration

#### Claims-Based Authorization

```csharp
config.Authorization = new AuthorizationConfiguration
{
    EnableClaimsCaching = true,
    ClaimsCacheExpiration = TimeSpan.FromHours(1),
    RefreshClaimsOnRoleChange = true,
    
    // Custom claim types
    PermissionClaimType = "permission",
    VenueClaimType = "venue",
    RoleClaimType = "role"
};
```

#### Permission Policies

The library automatically configures authorization policies based on your permission claims:

```csharp
// These are automatically configured
[Authorize(Policy = "read:venues")]
[Authorize(Policy = "write:venues")]
[Authorize(Policy = "admin:system")]
```

### Azure Maps Configuration

#### Service Registration

```csharp
config.AzureMaps = new AzureMapsConfiguration
{
    SubscriptionKey = "your_azure_maps_key",
    ClientId = "your_azure_maps_client_id", // For Azure AD authentication
    EnableCaching = true,
    CacheExpiration = TimeSpan.FromHours(24),
    
    // Geocoding settings
    DefaultCountry = "US",
    MaxResults = 10,
    EnableReverseGeocoding = true
};
```

#### Azure Maps Setup

1. **Create Azure Maps Account** in Azure Portal
2. **Get Subscription Key** from the Authentication section
3. **Configure CORS** if using from browser applications

### SignalR Configuration

#### Hub Settings

```csharp
config.SignalR = new SignalRConfiguration
{
    EnableDetailedErrors = builder.Environment.IsDevelopment(),
    ClientTimeoutInterval = TimeSpan.FromMinutes(1),
    KeepAliveInterval = TimeSpan.FromSeconds(15),
    HandshakeTimeout = TimeSpan.FromSeconds(15),
    MaximumReceiveMessageSize = 65536,
    
    // Scale-out configuration (for production)
    UseRedisBackplane = false,
    RedisConnectionString = "localhost:6379"
};
```

#### Client Configuration

JavaScript client setup:

```javascript
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/hubs/activitythread", {
        accessTokenFactory: () => getUserToken()
    })
    .withAutomaticReconnect()
    .build();

// Connect to activity thread updates
connection.start().then(function () {
    // Join venue-specific group
    connection.invoke("JoinVenueGroup", venueId);
}).catch(function (err) {
    console.error(err.toString());
});

// Listen for new posts
connection.on("NewPost", function (post) {
    updateActivityFeed(post);
});
```

### Logging Configuration

#### Serilog Setup

```csharp
config.Logging = new SerilogConfiguration
{
    MinimumLevel = LogEventLevel.Information,
    EnableConsoleLogging = true,
    EnableFileLogging = true,
    EnablePostgreSQLSink = true,
    
    // File logging settings
    FileLogPath = "./logs/pulse-.log",
    FileRollingInterval = RollingInterval.Day,
    FileRetainedFileCountLimit = 31,
    
    // PostgreSQL sink settings
    PostgreSQLTableName = "application_logs",
    PostgreSQLBatchPostingLimit = 50,
    PostgreSQLPeriod = TimeSpan.FromSeconds(5)
};
```

#### Custom Log Enrichers

```csharp
// Automatically configured enrichers
- Machine name
- Process ID
- Thread ID
- User ID (when authenticated)
- Venue context (when available)
```

### File Storage Configuration

#### Local Storage (Development)

```csharp
config.FileStorage = new FileStorageConfiguration
{
    Provider = FileStorageProvider.Local,
    LocalPath = "./uploads",
    MaxFileSize = 10 * 1024 * 1024, // 10MB
    AllowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".mp4", ".mov" },
    EnableImageResizing = true,
    ThumbnailSizes = new[] { 150, 300, 600 }
};
```

#### Azure Blob Storage (Production)

```csharp
config.FileStorage = new FileStorageConfiguration
{
    Provider = FileStorageProvider.AzureBlob,
    AzureBlobConnectionString = "your_storage_connection_string",
    AzureBlobContainerName = "pulse-uploads",
    EnableCDN = true,
    CDNEndpoint = "https://your-cdn-endpoint.azureedge.net",
    MaxFileSize = 50 * 1024 * 1024, // 50MB
    AllowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".mp4", ".mov", ".pdf" }
};
```

## Environment-Specific Configuration

### Development Environment

```json
{
  "PulseByMirthSystems": {
    "AutoMigrate": true,
    "SeedData": true,
    "Identity": {
      "RequireConfirmedEmail": false
    },
    "SignalR": {
      "EnableDetailedErrors": true
    },
    "Logging": {
      "MinimumLevel": "Debug",
      "EnableConsoleLogging": true
    }
  }
}
```

### Production Environment

```json
{
  "PulseByMirthSystems": {
    "AutoMigrate": false,
    "SeedData": false,
    "Identity": {
      "RequireConfirmedEmail": true,
      "RequireDigit": true,
      "RequiredLength": 8
    },
    "SignalR": {
      "EnableDetailedErrors": false,
      "UseRedisBackplane": true
    },
    "Logging": {
      "MinimumLevel": "Warning",
      "EnableConsoleLogging": false
    },
    "FileStorage": {
      "Provider": "AzureBlob"
    }
  }
}
```

## Advanced Configuration

### Custom Dependency Injection

```csharp
services.AddPulseByMirthSystems(config => { /* config */ })
       .AddScoped<ICustomService, CustomService>()
       .Configure<CustomOptions>(options => { /* custom options */ });
```

### Custom Authorization Policies

```csharp
services.AddAuthorization(options =>
{
    options.AddPolicy("CustomVenueAccess", policy =>
        policy.Requirements.Add(new VenueAccessRequirement()));
});
```

### Custom SignalR Events

```csharp
public class CustomHub : Hub<ICustomHubClient>
{
    public async Task SendCustomUpdate(string data)
    {
        await Clients.All.ReceiveCustomUpdate(data);
    }
}

// Register custom hub
app.MapHub<CustomHub>("/hubs/custom");
```

## Troubleshooting

### Common Issues

1. **Database Connection Issues**
   - Verify PostgreSQL is running
   - Check connection string format
   - Ensure database and user exist

2. **Migration Issues**
   - Clear bin/obj folders
   - Rebuild solution
   - Check EF Core tools version

3. **Identity Configuration Issues**
   - Verify cookie settings for web apps
   - Check JWT configuration for APIs
   - Ensure claims are properly populated

4. **SignalR Connection Issues**
   - Check authentication tokens
   - Verify CORS configuration
   - Check firewall/proxy settings

### Debug Configuration

Enable detailed logging for troubleshooting:

```json
{
  "Logging": {
    "LogLevel": {
      "PulseByMirthSystems": "Debug",
      "Microsoft.EntityFrameworkCore": "Information",
      "Microsoft.AspNetCore.SignalR": "Debug"
    }
  }
}
```

## Performance Tuning

### Database Performance

```csharp
config.Database = new DatabaseConfiguration
{
    CommandTimeout = 30,
    EnableSensitiveDataLogging = false,
    EnableServiceProviderCaching = true,
    MaxRetryCount = 3,
    MaxRetryDelay = TimeSpan.FromSeconds(30)
};
```

### Caching Configuration

```csharp
config.Caching = new CachingConfiguration
{
    EnableDistributedCache = true,
    DefaultExpiration = TimeSpan.FromMinutes(30),
    ClaimsExpiration = TimeSpan.FromHours(1),
    LookupDataExpiration = TimeSpan.FromHours(24)
};
```

This comprehensive configuration guide ensures optimal setup and performance of the PulseByMirthSystems library across all environments.
