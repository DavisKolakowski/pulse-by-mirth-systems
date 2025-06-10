# Pulse Project - GitHub Copilot Instructions

## Project Overview

**Pulse** is a real-time nightlife discovery platform built with .NET 9 and Aspire 9. It connects users with local venues through live activity threads, special offers, and community-driven content. The system uses a sophisticated database-driven authorization model with Auth0 for authentication only.

**Key Features:**
- **Three-Category Classification System:** Venue Types (fixed), Tags (for specials), and Vibes (user atmosphere descriptors)
- **Ephemeral Content Model:** All user posts expire after 15 minutes for real-time relevance
- **Privacy-First Location:** Manual address entry as default, opt-in location services
- **Special-Driven Discovery:** Users find venues primarily through active specials and promotions
- **Live Activity Threads:** Social media-style comment threads for each venue with real-time updates
- **PostgreSQL-Based Recommendations:** Traditional database algorithms for MVP, AI-powered engine planned for V2

**Project Lead:** Davis Kolakowski  
**Current Status:** Database authorization system implementation phase

## Business Features & User Experience

### Core User Journey
1. **Location Selection:** Users manually enter address or opt-in to location services
2. **Venue Discovery:** Browse venues by Types, Tags, or current Vibes within selected radius
3. **Real-Time Activity:** View live activity threads showing what's happening now
4. **Special Engagement:** Discover and engage with time-sensitive venue specials
5. **Content Creation:** Post photos, videos, and comments that auto-expire in 15 minutes
6. **Community Interaction:** Add Vibes to posts, follow Tags for notifications

### Three-Category Classification System

#### Venue Categories
- **Purpose:** Basic venue classification and filtering
- **Examples:** Bar, Restaurant, Cafe, Lounge, Club, Brewery, etc.
- **Implementation:** Venue owners select one category.
- **Database:** `venue_categories` table with predefined list

#### Tags (Special Discovery)
- **Purpose:** Event/promotion identifiers for specials discovery
- **Format:** Preceded by # symbol (#happyhour, #wingsnight, #liveband)
- **Features:** Can be followed, searched, filtered; performance analytics available
- **Database:** `tags` table with color, icon, usage count tracking

#### Vibes (Atmosphere Descriptors)
- **Purpose:** User-generated current atmosphere descriptions
- **Format:** Preceded by # symbol (#busy, #quiet, #goodvibes, #livemusic)
- **Lifecycle:** Temporary, tied to 15-minute post expiration
- **Database:** `vibes` table with many-to-many relationship to posts

### Special Management System
- **Timing Control:** Start/end dates, daily time windows, recurring schedules
- **Categories:** Food, Drink, Entertainment (Food/Drink/Entertainment)
- **Cron Scheduling:** Complex recurrence patterns (e.g., "0 17 * * 1-5" for weekdays)
- **Tag Association:** Multiple tags per special for discovery
- **Real-Time Updates:** Active specials appear in feeds, vanish when expired

### Live Activity Threads
- **Content Types:** Text comments, photos, short video clips
- **Expiration:** All user content auto-expires after 15 minutes
- **Vibe Integration:** Users add atmosphere descriptors to posts
- **Venue Interaction:** Venues can monitor but not delete user posts
- **Community Features:** Real-time updates, activity indicators on venue cards

### User Follow & Notification System
- **Tag Following:** Users follow specific tags for special notifications
- **Notification Types:** New specials, special reminders, venue activity
- **Preferences:** Granular control over notification types
- **Database:** `user_follows` table with follow type enum

### Analytics & Insights
- **Venue Analytics:** Profile views, special engagement, tag performance
- **User Demographics:** Anonymized user behavior patterns
- **Tag Trends:** Popular tags, seasonal patterns, geographic variations
- **Special Performance:** Click-through rates, conversion tracking

### Monetization Features
- **Free Tier:** Basic profile management, standard updates, essential analytics
- **Premium Tier:** Featured placement, push notifications, advanced analytics, unlimited tags
- **Revenue Model:** 10-15% conversion rate target for venues

### Backend (.NET 9)
- **Framework:** ASP.NET Core 9.0 with Aspire 9.3.0
- **Database:** PostgreSQL 16 with PostGIS 3.4 (Geographic data support)
- **ORM:** Entity Framework Core 9.0.5 with:
  - `Npgsql.EntityFrameworkCore.PostgreSQL` 9.0.4
  - `Npgsql.EntityFrameworkCore.PostgreSQL.NetTopologySuite` 9.0.4 (Spatial data)
  - `Npgsql.EntityFrameworkCore.PostgreSQL.NodaTime` 9.0.4 (Date/time handling)
  - `EFCore.NamingConventions` 9.0.0 (Snake case for PostgreSQL)
- **Authentication:** Auth0 with JWT Bearer tokens
- **Authorization:** Custom database-driven RBAC system
- **API Architecture:** Hybrid GraphQL + REST API in single project
- **GraphQL Implementation:** GraphQL.NET - Industry standard for .NET
  - `GraphQL` - Core GraphQL execution engine
  - `GraphQL.Server.All` - ASP.NET Core integration
  - `GraphQL.Authorization` - Authorization support
  - `GraphQL.DataLoader` - Efficient data loading
  - **Documentation:** https://graphql-dotnet.github.io/
  - **Why GraphQL.NET:** Mature, stable, Facebook GraphQL spec compliant, extensive ecosystem, works alongside REST endpoints
- **Time Handling:** NodaTime 3.2.2 for precise date/time operations
- **Spatial Data:** NetTopologySuite for geographic calculations
- **Testing:** NUnit framework

### Frontend (React + TypeScript)
- **Framework:** React 19 with TypeScript
- **Build Tool:** Vite (fast development and optimized builds)
- **Router:** React Router v7.6.2 for client-side routing
- **State Management:** To be implemented (Redux planned for predictable state container)
- **UI Components:** Currently using custom CSS (Hero-UI with TailwindCSS planned for final design)
- **Styling:** Custom CSS with gradient themes and responsive design
- **Authentication:** Auth0 React SDK (@auth0/auth0-react v2.2.4)
- **HTTP Client:** Axios v1.7.9 for API communication
- **GraphQL Client:** Apollo Client or urql planned for GraphQL integration

### Infrastructure & DevOps
- **Container Orchestration:** .NET Aspire 9.3.0
- **Database Container:** PostGIS/PostGIS 16-3.4 Docker image
- **Development Environment:** Aspire dashboard at `https://localhost:17190`
- **API Port:** 6060 (configured in Aspire)
- **Frontend Port:** 4040

### Azure Services (Current & Future Integration)
- **Maps Service:** Azure Maps (preview packages required for .NET):
  - `Azure.Maps.Common` - Core Azure Maps functionality
  - `Azure.Maps.Geolocation` - IP-based geolocation services
  - `Azure.Maps.Rendering` - Map tile rendering and static maps
  - `Azure.Maps.Routing` - Route calculation and navigation
  - `Azure.Maps.Search` - Geocoding, reverse geocoding, POI search
  - `Azure.Maps.TimeZones` - Time zone lookup by coordinates
  - `Azure.ResourceManager.Maps` - Azure Maps account management
  - **Documentation:** https://learn.microsoft.com/en-us/dotnet/api/overview/azure/maps?view=azure-dotnet-preview

### Future Technology Stack (Aspire Integrations)

#### Caching Solutions
- **Garnet:** `Aspire.StackExchange.Redis` with Garnet - Microsoft's Redis replacement (Primary Choice)
- **Documentation:** https://learn.microsoft.com/en-us/dotnet/aspire/caching/stackexchange-redis-integration?pivots=garnet

#### Messaging Systems
**For notifications and temporary special menu comments:**
- **Apache Kafka:** `Aspire.Confluent.Kafka` - High-throughput event streaming
- **RabbitMQ:** `Aspire.RabbitMQ.Client` - Traditional message queuing
- **Documentation:** 
  - Kafka: https://learn.microsoft.com/en-us/dotnet/aspire/messaging/kafka-integration
  - RabbitMQ: https://learn.microsoft.com/en-us/dotnet/aspire/messaging/rabbitmq-integration

#### Document Storage & Recommendation Engine
**Choose based on use case:**
- **MongoDB:** `Aspire.MongoDB.Driver` - General document storage
- **RavenDB:** `Aspire.CommunityToolkit.RavenDB` - .NET-native document database
- **Qdrant:** `Aspire.Qdrant.Client` - Vector database for recommendation engine
- **Documentation:**
  - MongoDB: https://learn.microsoft.com/en-us/dotnet/aspire/database/mongodb-integration
  - RavenDB: https://learn.microsoft.com/en-us/dotnet/aspire/community-toolkit/ravendb
  - Qdrant: https://learn.microsoft.com/en-us/dotnet/aspire/database/qdrant-integration

#### Custom Aspire Integrations
**For specialized requirements:**
- **Custom Hosting:** https://learn.microsoft.com/en-us/dotnet/aspire/extensibility/custom-hosting-integration
- **Custom Client:** https://learn.microsoft.com/en-us/dotnet/aspire/extensibility/custom-client-integration
- **Secure Communication:** https://learn.microsoft.com/en-us/dotnet/aspire/extensibility/secure-communication-between-integrations

## Project Structure

```
g:\Dev\Projects\pulse\
├── src/
│   ├── Aspire/                          # Aspire orchestration
│   │   ├── Aspire.AppHost/              # Main orchestration host
│   │   └── Aspire.ServiceDefaults/      # Shared service configurations
│   ├── Pulse.API/                       # ASP.NET Core API
│   ├── Pulse.Core/                      # Core business logic and data (Authorization, Data, Extensions, Middlewares, Services)
│   ├── Pulse.Clients.Web/              # React TypeScript frontend
│   ├── Pulse.Services.DatabaseMigrations/ # EF Core migrations service
│   └── Tests/                           # Test projects
│       ├── Aspire.IntegrationTests/     # Aspire integration testing
│       └── Pulse.UnitTests/             # NUnit unit tests
├── docs/                                # Comprehensive documentation
└── .github/                             # GitHub configurations
```

**Note:** Project folders will expand with additional services, components, and features as the platform grows.

## Architecture Principles

### GraphQL Server Choice Analysis

**GraphQL.NET vs. Alternatives:**
- **GraphQL.NET** ✅ **CHOSEN**
  - **Pros:** Mature, stable, Facebook GraphQL spec compliant, extensive ecosystem, works alongside REST endpoints, flexible schema definition, excellent community support
  - **Cons:** More manual setup than code-first alternatives, requires more boilerplate
  - **Best for:** Production applications requiring full GraphQL spec compliance, hybrid REST+GraphQL APIs

- **HotChocolate (ChilliCream)** ⚠️ **NOT CHOSEN**
  - **Pros:** Code-first approach, excellent EF Core integration, built-in projections/filtering, active development
  - **Cons:** Vendor lock-in, less mature ecosystem, potential licensing concerns for commercial use
  - **Best for:** Complex schemas, enterprise applications with ChilliCream ecosystem

- **Entity GraphQL** ⚠️ **NOT CHOSEN**
  - **Pros:** Extremely simple EF Core integration, minimal setup
  - **Cons:** Limited to Entity Framework, less flexibility, smaller ecosystem
  - **Best for:** Basic CRUD operations over existing EF models

**Decision Rationale:**
GraphQL.NET chosen for Pulse because it provides the industry standard implementation, works seamlessly alongside REST endpoints in the same project, and offers maximum flexibility for our hybrid API architecture without vendor dependencies.

### Clean Architecture Implementation ✅
The project follows **Clean Architecture** patterns with clear separation of concerns:

#### Layer Structure
```
Pulse.Core/           # Domain + Application Layer
├── Data/            # Entities, Configurations, DbContext  
├── Services/        # Application Services (Business Logic)
├── Authorization/   # Domain-specific authorization logic
├── Extensions/      # Dependency injection configuration
└── Middlewares/     # Cross-cutting concerns

Pulse.API/           # Infrastructure Layer (Web)
├── Controllers/     # API endpoints (thin controllers)
└── Program.cs       # Dependency injection & middleware pipeline

Tests/               # Test Layer
├── Pulse.UnitTests/        # Domain & Application logic tests
└── Aspire.IntegrationTests/ # Infrastructure integration tests
```

#### Development Principles (MANDATORY)

**SOLID Principles:**
- **S**ingle Responsibility Principle - Each class has one reason to change
- **O**pen/Closed Principle - Open for extension, closed for modification
- **L**iskov Substitution Principle - Derived classes must be substitutable
- **I**nterface Segregation Principle - No client should depend on unused methods
- **D**ependency Inversion Principle - Depend on abstractions, not concretions

**DRY (Don't Repeat Yourself):**
- Extract common logic into shared services
- Use extension methods for repeated patterns
- Create reusable components and utilities

**Additional Principles:**
- **KISS** (Keep It Simple, Stupid) - Prefer simple solutions
- **YAGNI** (You Aren't Gonna Need It) - Don't build premature features
- **Fail Fast** - Validate early and throw meaningful exceptions
- **Dependency Injection** - All dependencies injected through constructor

#### Repository + Service Pattern ✅
```csharp
// Repository Layer (Data Access)
public interface IVenueRepository
{
    Task<IEnumerable<Venue>> GetNearbyVenuesAsync(Point location, double radiusMeters);
    Task<Venue?> GetByIdAsync(long id);
    Task<Venue> CreateAsync(Venue venue);
    Task UpdateAsync(Venue venue);
    Task DeleteAsync(long id);
}

// Service Layer (Business Logic)
public interface IVenueService
{
    Task<VenueDto> CreateVenueAsync(CreateVenueRequest request);
    Task<IEnumerable<VenueDto>> FindNearbyVenuesAsync(double latitude, double longitude, double radiusKm);
    Task<VenueDto?> GetVenueDetailsAsync(long venueId);
}

// Controller Layer (API Interface - Keep Thin!)
[ApiController]
[Route("api/[controller]")]
public class VenuesController : ControllerBase
{
    private readonly IVenueService _venueService;
    
    [HttpGet("nearby")]
    public async Task<ActionResult<IEnumerable<VenueDto>>> GetNearbyVenues(
        [FromQuery] double lat, [FromQuery] double lng, [FromQuery] double radius = 5.0)
    {
        var venues = await _venueService.FindNearbyVenuesAsync(lat, lng, radius);
        return Ok(venues);
    }
}
```

#### Dependency Injection Pattern
```csharp
// Interface definitions in Pulse.Core
public interface IUserService { }
public interface IAuthorizationService { }
public interface IVenueRepository { }

// Implementations in Pulse.Core
public class DatabaseAuthorizationService : IAuthorizationService { }
public class VenueService : IVenueService { }

// Registration in ServiceCollectionExtensions.cs
public static IServiceCollection AddCoreServices(this IServiceCollection services)
{
    // Repositories
    services.AddScoped<IVenueRepository, VenueRepository>();
    services.AddScoped<IUserRepository, UserRepository>();
    
    // Services  
    services.AddScoped<IVenueService, VenueService>();
    services.AddScoped<IAuthorizationService, DatabaseAuthorizationService>();
    
    return services;
}
```

### Authentication vs Authorization (CRITICAL)
The project uses a **hybrid approach** that separates concerns:

#### Authentication (Auth0) ✅
- **Purpose:** Identity verification only
- **Responsibilities:** Login, JWT tokens, user identity
- **What Auth0 Provides:** `sub` claim (user identifier) only
- **Configuration:** Minimal - no permissions, roles, or Management API

#### Authorization (Database) ✅
- **Purpose:** All permission checking and role management
- **Responsibilities:** Roles, permissions, venue-specific access control
- **Implementation:** PostgreSQL with custom `DatabaseAuthorizationService`
- **Benefits:** Performance, flexibility, cost savings, audit trail

### Database Schema Design

#### Core Authorization Entities
```sql
-- Users linked to Auth0 by provider_id (Auth0 'sub' claim)
users (provider_id, email, first_name, last_name, is_active)

-- Four predefined roles
roles (name: Administrator, ContentManager, VenueOwner, VenueManager)

-- 35 granular permissions (action:resource pattern)
permissions (name, category, action, resource)

-- Role-permission mappings
role_permissions (role_id, permission_id)

-- Global role assignments
user_roles (user_id, role_id, assigned_at, is_active)

-- Venue-specific role assignments
venue_roles (user_id, venue_id, role_id, assigned_at, is_active)
```

#### Naming Conventions
- **Database:** Snake case (`user_roles`, `venue_categories`)
- **C# Entities:** PascalCase (`UserRole`, `VenueCategory`)
- **EF Configuration:** `EFCore.NamingConventions` handles conversion

### Permission System

#### Permission Pattern: `{action}:{resource}`
**Actions:** `read`, `write`, `delete`, `moderate`, `upload`, `manage`, `admin`, `config`
**Resources:** `venues`, `specials`, `content`, `posts`, `media`, `analytics`, etc.

#### Role Hierarchy
1. **Administrator** (Global) - All 35 permissions platform-wide
2. **Content Manager** (Global) - Content moderation and analytics
3. **Venue Owner** (Venue-specific) - Full venue management rights
4. **Venue Manager** (Venue-specific) - Content and special management

#### Authorization Flow
```csharp
// 1. Extract Auth0 user ID from JWT
var auth0UserId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

// 2. Look up user in database
var user = await _authService.GetUserByProviderIdAsync(auth0UserId);

// 3. Check permissions (global OR venue-specific)
var hasPermission = await _authService.HasPermissionAsync(user.Id, "read:venues", venueId);
```

## Current Implementation Status

### ✅ Completed (Database Schema Phase)
- Complete EF Core entities and configurations
- Initial migration with all tables created
- Seeded data: 4 roles, 35 permissions, admin user (Davis)
- Snake case naming convention for PostgreSQL
- PostGIS extension enabled for geographic data
- Aspire integration working with dashboard accessible
- React 19 frontend with Auth0 authentication implemented
- Basic responsive design with custom CSS
- Mobile navigation and PWA-ready foundation

### 🚧 Current Phase (Database Authorization Implementation)
**Priority Tasks:**
1. Remove Auth0 Management API dependencies ✅ (Already done - using authentication only)
2. Implement `DatabaseAuthorizationService`
3. Update `RBACHandler` to use database instead of JWT claims
4. Test complete authentication + authorization flow
5. Update frontend to use database permissions instead of Auth0 permission checks
6. Migrate from custom CSS to Hero-UI + TailwindCSS design system
7. Implement Redux state management
8. Add GraphQL client integration

### 📋 Implementation Plan Reference
See `docs/Implementation-Plan-Database-Authorization.md` for detailed step-by-step plan.

## Coding Guidelines

### Entity Framework Core Patterns
```csharp
// Always use NodaTime for temporal data
public Instant CreatedAt { get; set; }
public Instant? UpdatedAt { get; set; }

// Use NetTopologySuite for geographic data
public Point Location { get; set; } // PostGIS geometry

// Follow soft delete pattern
public bool IsActive { get; set; } = true;
public Instant? DeactivatedAt { get; set; }
```

### Authorization Implementation
```csharp
// GraphQL.NET Query with Authorization
public class VenueQuery : ObjectGraphType
{
    public VenueQuery(IVenueRepository repository, IAuthorizationService authService)
    {
        Field<ListGraphType<VenueType>>("venues")
            .Description("Get venues near a location")
            .Argument<FloatGraphType>("latitude", "Latitude coordinate")
            .Argument<FloatGraphType>("longitude", "Longitude coordinate")
            .Argument<FloatGraphType>("radiusKm", "Search radius in kilometers", defaultValue: 5.0)
            .AuthorizeWithPolicy("read:venues")
            .ResolveAsync(async context =>
            {
                var latitude = context.GetArgument<double?>("latitude");
                var longitude = context.GetArgument<double?>("longitude");
                var radiusKm = context.GetArgument<double>("radiusKm");

                if (latitude.HasValue && longitude.HasValue)
                {
                    var location = new Point(longitude.Value, latitude.Value) { SRID = 4326 };
                    return await repository.GetNearbyVenuesAsync(location, radiusKm * 1000);
                }
                return await repository.GetAllVenuesAsync();
            });
    }
}

// GraphQL.NET Mutation with Authorization
public class VenueMutation : ObjectGraphType
{
    public VenueMutation(IVenueService venueService)
    {
        Field<VenueType>("createVenue")
            .Description("Create a new venue")
            .Argument<NonNullGraphType<CreateVenueInputType>>("input", "Venue details")
            .AuthorizeWithPolicy("write:venues")
            .ResolveAsync(async context =>
            {
                var input = context.GetArgument<CreateVenueInput>("input");
                return await venueService.CreateVenueAsync(input);
            });
    }
}

// GraphQL.NET Subscription for Real-time Updates
public class VenueSubscription : ObjectGraphType
{
    public VenueSubscription(IEventAggregator eventAggregator)
    {
        Field<VenueActivityType>("venueActivity")
            .Description("Subscribe to venue activity updates")
            .Argument<NonNullGraphType<LongGraphType>>("venueId", "Venue identifier")
            .AuthorizeWithPolicy("read:venues")
            .ResolveStream(context =>
            {
                var venueId = context.GetArgument<long>("venueId");
                return eventAggregator.GetStream<VenueActivity>($"venue_{venueId}");
            });
    }
}

// REST Fallback Controller (for simple operations)
[ApiController]
[Route("api/[controller]")]
public class VenuesController : ControllerBase
{
    [HttpGet("health")]
    public IActionResult HealthCheck() => Ok(new { status = "healthy" });
}

// GraphQL types work seamlessly with database authorization
// Permission checking happens in DatabaseAuthorizationService
// Auth0 only provides authentication (JWT with 'sub' claim)
// All authorization decisions come from PostgreSQL database
```

### Service Registration Pattern
```csharp
// In ServiceCollectionExtensions.cs
public static IServiceCollection AddCoreServices(this IServiceCollection services)
{
    services.AddScoped<IAuthorizationService, DatabaseAuthorizationService>();
    services.AddMemoryCache(); // For permission caching (will migrate to Garnet)
    
    // GraphQL Services (GraphQL.NET)
    services.AddSingleton<IDocumentExecuter, DocumentExecuter>();
    services.AddSingleton<IDocumentWriter, DocumentWriter>();
    services.AddSingleton<ISchema, PulseSchema>();
    
    // GraphQL Types
    services.AddTransient<VenueQuery>();
    services.AddTransient<VenueMutation>();
    services.AddTransient<VenueSubscription>();
    services.AddTransient<VenueType>();
    services.AddTransient<CreateVenueInputType>();
    
    // Configure schema
    services.AddSingleton<ISchema>(provider =>
    {
        var schema = new PulseSchema(new SelfActivatingServiceProvider(provider));
        schema.Query = provider.GetRequiredService<VenueQuery>();
        schema.Mutation = provider.GetRequiredService<VenueMutation>();
        schema.Subscription = provider.GetRequiredService<VenueSubscription>();
        return schema;
    });
    
    return services;
}
```

## Database Configuration

### Connection String Pattern
```csharp
// Aspire handles connection string management
builder.AddNpgsqlDbContext<ApplicationDbContext>("pulsedb", options =>
{
    options.UseNpgsql(npgsqlOptions =>
    {
        npgsqlOptions.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
        npgsqlOptions.UseNodaTime();           // Enable NodaTime support
        npgsqlOptions.UseNetTopologySuite();   // Enable spatial data support
    });

    options.UseSnakeCaseNamingConvention();
});
```

### Required PostgreSQL Extensions
```sql
-- Automatically configured in ApplicationDbContext
postgis, postgis_raster, postgis_sfcgal, postgis_topology, postgis_tiger_geocoder
address_standardizer, address_standardizer_data_us, fuzzystrmatch, plpgsql
```

## Testing Strategy

### Testing Requirements (MANDATORY)
**CRITICAL RULE:** Every single piece of code written must have corresponding tests:

- **Unit Tests** → `Tests/Pulse.UnitTests/` using NUnit framework
- **Integration Tests** → `Tests/Aspire.IntegrationTests/` for end-to-end scenarios
- **Test Coverage:** Target minimum 80% code coverage
- **Test-Driven Development:** Write tests before or alongside implementation

### Test Categories

#### Unit Tests (Pulse.UnitTests)
```csharp
[TestFixture]
public class DatabaseAuthorizationServiceTests
{
    private ApplicationDbContext _context;
    private DatabaseAuthorizationService _service;
    
    [SetUp]
    public void Setup()
    {
        // In-memory database setup for unit tests
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new ApplicationDbContext(options);
        _service = new DatabaseAuthorizationService(_context, Mock.Of<IMemoryCache>());
    }
    
    [Test]
    public async Task HasPermissionAsync_AdminUser_ReturnsTrue()
    {
        // Arrange
        await SeedTestData();
        
        // Act
        var hasPermission = await _service.HasPermissionAsync(1, "read:venues");
        
        // Assert
        Assert.IsTrue(hasPermission);
    }
}
```

#### Integration Tests (Aspire.IntegrationTests)
```csharp
[TestFixture]
public class AuthorizationIntegrationTests
{
    private HttpClient _client;
    private DistributedApplication _app;
    
    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        var appHost = await DistributedApplicationTestingBuilder
            .CreateAsync<Projects.Aspire_AppHost>();
        _app = await appHost.BuildAsync();
        await _app.StartAsync();
        _client = _app.CreateHttpClient("pulse-api");
    }
    
    [Test]
    public async Task GetVenues_WithValidToken_ReturnsSuccess()
    {
        // Test complete authentication + authorization flow
        _client.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue("Bearer", GetValidJwtToken());
            
        var response = await _client.GetAsync("/api/venues");
        
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    }
}
```

#### Test Patterns for Different Components

**Service Tests:**
- Mock dependencies using `Moq` or `NSubstitute`
- Use in-memory database for data access tests
- Test all business logic branches
- Test error handling and edge cases

**Controller Tests:**
- Test authorization policies
- Test request/response mapping
- Test validation logic
- Mock service dependencies

**Authorization Tests:**
- Test permission checking logic
- Test venue-specific authorization
- Test role hierarchy
- Test Auth0 integration

**Geographic/Spatial Tests:**
- Test PostGIS spatial queries
- Test location-based searches
- Test distance calculations
- Test coordinate transformations

### Testing Guidelines

#### What to Test (MANDATORY)
✅ **All Services** - Every public method must have unit tests
✅ **All Controllers** - Every action method must have tests
✅ **Authorization Logic** - Every permission check must be tested
✅ **Database Queries** - All EF Core queries must be tested
✅ **Middleware** - All custom middleware must be tested
✅ **Extensions** - All extension methods must be tested
✅ **API Endpoints** - All endpoints must have integration tests
✅ **Business Logic** - All domain logic must be tested

#### Test Naming Convention
```csharp
// Pattern: MethodName_Scenario_ExpectedResult
[Test]
public async Task HasPermissionAsync_UserWithGlobalRole_ReturnsTrue() { }

[Test]
public async Task HasPermissionAsync_UserWithoutPermission_ReturnsFalse() { }

[Test]
public async Task GetVenuesAsync_WithValidLocation_ReturnsNearbyVenues() { }
```

#### Test Data Management
```csharp
public static class TestDataBuilder
{
    public static User CreateAdminUser() => new User
    {
        Id = 1,
        ProviderId = "auth0|test-admin",
        Email = "test@example.com",
        FirstName = "Test",
        LastName = "Admin",
        IsActive = true,
        CreatedAt = SystemClock.Instance.GetCurrentInstant()
    };
    
    public static Role CreateAdministratorRole() => new Role
    {
        Id = 1,
        Name = "Administrator",
        Description = "Full system access",
        IsActive = true
    };
}
```

### GraphQL Testing
```csharp
[TestFixture]
public class GraphQLVenueQueriesTests
{
    [Test]
    public async Task GetVenues_WithLocationFilter_ReturnsNearbyVenues()
    {
        // Test GraphQL query with spatial filtering
        var query = """
            query GetNearbyVenues($lat: Float!, $lng: Float!, $radius: Float!) {
                venues(latitude: $lat, longitude: $lng, radiusKm: $radius) {
                    id
                    name
                    location
                    distance
                }
            }
            """;
            
        var result = await _client.PostGraphQLAsync(query, new { lat = 40.7128, lng = -74.0060, radius = 5.0 });
        
        Assert.IsTrue(result.Data.venues.Count > 0);
    }
}

[TestFixture]
public class GraphQLAuthorizationTests
{
    [Test]
    public async Task CreateVenue_WithoutPermission_ReturnsAuthorizationError()
    {
        // Test GraphQL mutation authorization
        var mutation = """
            mutation CreateVenue($input: CreateVenueInput!) {
                createVenue(input: $input) {
                    id
                    name
                }
            }
            """;
            
        var result = await _client.PostGraphQLAsync(mutation, new { input = new { name = "Test Venue" } });
        
        Assert.IsTrue(result.Errors?.Any(e => e.Extensions["code"]?.ToString() == "AUTH_NOT_AUTHORIZED"));
    }
}
```
```csharp
[Test]
public async Task HasPermissionAsync_AdminUser_ReturnsTrue()
{
    // Davis (Administrator) should have all permissions
    var hasPermission = await _authService.HasPermissionAsync(1, "read:venues");
    Assert.IsTrue(hasPermission);
}

[Test]
public async Task HasPermissionAsync_VenueSpecific_ReturnsTrue()
{
    // Venue Manager should have permission for assigned venue only
    var hasPermission = await _authService.HasPermissionAsync(2, "write:specials", venueId: 1);
    Assert.IsTrue(hasPermission);
}

[Test]
public async Task HasPermissionAsync_VenueSpecific_WrongVenue_ReturnsFalse()
{
    // Venue Manager should NOT have permission for unassigned venue
    var hasPermission = await _authService.HasPermissionAsync(2, "write:specials", venueId: 999);
    Assert.IsFalse(hasPermission);
}
```

### GraphQL Testing
```csharp
[TestFixture]
public class GraphQLVenueQueriesTests
{
    [Test]
    public async Task GetVenues_WithLocationFilter_ReturnsNearbyVenues()
    {
        // Test GraphQL query with spatial filtering
        var query = """
            query GetNearbyVenues($lat: Float!, $lng: Float!, $radius: Float!) {
                venues(latitude: $lat, longitude: $lng, radiusKm: $radius) {
                    id
                    name
                    location
                    distance
                }
            }
            """;
            
        var result = await _client.PostGraphQLAsync(query, new { lat = 40.7128, lng = -74.0060, radius = 5.0 });
        
        Assert.IsTrue(result.Data.venues.Count > 0);
    }
}

[TestFixture]
public class GraphQLAuthorizationTests
{
    [Test]
    public async Task CreateVenue_WithoutPermission_ReturnsAuthorizationError()
    {
        // Test GraphQL mutation authorization
        var mutation = """
            mutation CreateVenue($input: CreateVenueInput!) {
                createVenue(input: $input) {
                    id
                    name
                }
            }
            """;
            
        var result = await _client.PostGraphQLAsync(mutation, new { input = new { name = "Test Venue" } });
        
        Assert.IsTrue(result.Errors?.Any(e => e.Extensions["code"]?.ToString() == "AUTH_NOT_AUTHORIZED"));
    }
}
```

## Important Context for Copilot

### What NOT to Suggest
❌ **Monolithic controller logic** - Keep controllers thin, move logic to services
❌ **Direct DbContext in controllers** - Use repository and service patterns
❌ **Concrete dependencies** - Always use interfaces for dependency injection
❌ **Code duplication** - Follow DRY principle, extract common logic
❌ **Violation of SRP** - One class should have one responsibility
❌ **Magic numbers/strings** - Use constants or configuration
❌ **Auth0 Management API usage** - We've moved away from this
❌ **Auth0 permissions in JWT tokens** - Database handles all authorization
❌ **Synchronization between Auth0 and database** - Auth0 is authentication only
❌ **System.DateTime** - Always use NodaTime.Instant
❌ **Manual SQL queries** - Use EF Core with proper navigation properties
❌ **Code without corresponding tests** - Every implementation needs tests
❌ **Hardcoded configuration values** - Use appsettings.json and configuration binding
❌ **Direct database calls in controllers** - Use service layer pattern
❌ **Missing error handling** - All external calls must have proper error handling

### What TO Suggest
✅ **Clean Architecture patterns** - Repository + Service + Controller layers
✅ **SOLID principles** - Single responsibility, dependency inversion, etc.
✅ **DRY principle** - Extract common logic, avoid repetition
✅ **Interface-based design** - All services implement interfaces
✅ **Thin controllers** - Business logic in services, not controllers
✅ **Repository pattern** - Data access abstraction layer
✅ **Service layer** - Business logic encapsulation
✅ **Constructor injection** - All dependencies injected via constructor
✅ **Meaningful exceptions** - Custom exceptions with clear messages
✅ **Input validation** - Validate at service boundaries
✅ **GraphQL.NET query design** - Efficient data fetching with GraphQL.NET
✅ **GraphQL authorization** - Permission-based field and type access
✅ **GraphQL.NET schema patterns** - Proper type definitions and resolvers
✅ **GraphQL subscriptions** - Real-time updates for live activity feeds
✅ **REST fallback endpoints** - Simple operations and health checks
✅ **Database authorization patterns** with `IAuthorizationService`
✅ **NodaTime for all temporal operations**
✅ **NetTopologySuite for geographic calculations**
✅ **Aspire integration patterns for new services**
✅ **EF Core navigation properties and LINQ queries**
✅ **Proper venue context extraction in authorization handlers**
✅ **Snake case naming for new database entities**
✅ **Unit tests for all service methods**
✅ **Integration tests for all API endpoints**
✅ **Test-driven development approaches**
✅ **Dependency injection patterns**
✅ **Configuration-based settings management**
✅ **Proper error handling and logging**
✅ **Performance considerations (caching, async/await)**
✅ **Azure Maps integration for geographic features**

### Key Files to Reference
- `src/Pulse.Core/Data/ApplicationDbContext.cs` - Main EF context
- `src/Pulse.Core/Authorization/RBACHandler.cs` - Authorization logic
- `src/Pulse.Core/Authorization/PermissionConstants.cs` - Permission definitions
- `docs/Database-Only-Authorization-Final.md` - Complete authorization guide
- `docs/Implementation-Plan-Database-Authorization.md` - Current implementation plan

### Common Patterns
```csharp
// User lookup pattern
var user = await _context.Users
    .FirstOrDefaultAsync(u => u.ProviderId == auth0UserId);

// Permission checking with venue context
var hasVenuePermission = await _context.VenueRoles
    .Where(vr => vr.UserId == userId && vr.VenueId == venueId && vr.IsActive)
    .SelectMany(vr => vr.Role.RolePermissions)
    .Any(rp => rp.Permission.Name == permission);

// Geographic queries with PostGIS
var nearbyVenues = await _context.Venues
    .Where(v => v.Location.IsWithinDistance(userLocation, distanceInMeters))
    .ToListAsync();
```

## Environment Setup

### Required Tools
- .NET 9 SDK
- Docker Desktop (for PostgreSQL with PostGIS)
- Node.js 18+ (for frontend)
- Visual Studio 2022 or VS Code with C# Dev Kit

### Development Workflow
1. Start Aspire host: `dotnet run --project src/Aspire/Aspire.AppHost`
2. Access Aspire dashboard: `https://localhost:17190`
3. API automatically available at configured port
4. Database migrations run automatically via dedicated service

This comprehensive guide should help GitHub Copilot provide contextually accurate suggestions aligned with your project's architecture and current implementation phase.
