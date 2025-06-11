# Pulse Clean Architecture Library Implementation Plan

## Overview
We are refactoring the existing Pulse application into a comprehensive, testable library using Clean Architecture principles. The library will be self-contained in `PulseByMirthSystems` and consumed via extension methods.

## Architecture Goals

### Core Principles
1. **Clean Architecture**: Domain at center, dependencies pointing inward
2. **High Test Coverage**: Target 90%+ code coverage with comprehensive unit tests
3. **Dependency Injection**: Everything injectable, especially IClock for time operations
4. **Database-First Entities**: No enums in database, use lookup tables instead
5. **Extension-Based Consumption**: Simple setup via `AddPulseByMirthSystems()` and `AddPulseByMirthSystemsEndpoints()`
6. **Microsoft Identity Integration**: Fully extend ASP.NET Core Identity (no Auth0 provider separation)
7. **One Class Per File**: Each class, interface, enum, record, and model gets its own file for organization

### Technology Stack
- **.NET 9**
- **Entity Framework Core 9** with PostgreSQL
- **ASP.NET Core Identity** (extended, not separate) for authentication
- **Serilog** for logging
- **NodaTime** for time operations
- **Azure Maps** for geolocation services (geocoding/reverse geocoding only)
- **NetTopologySuite** for spatial data
- **SignalR** for real-time features
- **Local File Storage** for Phase 1 (abstract for future Azure Blob)
- **XUnit** for testing

## Clean Architecture Folder Structure

```
PulseByMirthSystems/
├── Domain/                          # Core business logic (no dependencies)
│   ├── Entities/                    # Business entities
│   ├── ValueObjects/               # Value objects
│   ├── Enums/                      # Domain enums (not database enums)
│   ├── Interfaces/                 # Domain service interfaces
│   ├── Events/                     # Domain events
│   └── Exceptions/                 # Domain-specific exceptions
├── Application/                     # Use cases and application logic
│   ├── Common/                     # Shared application concerns
│   │   ├── Interfaces/             # Application service interfaces
│   │   ├── Behaviors/              # Cross-cutting behaviors
│   │   ├── Mappings/               # Object mappings
│   │   └── Models/                 # DTOs and view models
│   ├── Features/                   # Feature-based organization
│   │   ├── Venues/                 # Venue management
│   │   ├── Users/                  # User management
│   │   ├── Specials/               # Special offers
│   │   ├── Posts/                  # Social posts
│   │   ├── ActivityThreads/        # Real-time activity
│   │   └── Notifications/          # Notifications
│   └── Services/                   # Application services
├── Infrastructure/                  # External concerns
│   ├── Data/                       # EF Core implementation
│   │   ├── Context/                # DbContext
│   │   ├── Configurations/         # Entity configurations
│   │   ├── Repositories/           # Repository implementations
│   │   ├── Interceptors/           # EF interceptors
│   │   └── Migrations/             # Database migrations
│   ├── Identity/                   # ASP.NET Core Identity
│   │   ├── Models/                 # Identity models
│   │   ├── Services/               # Identity services
│   │   └── Stores/                 # Custom identity stores
│   ├── Authorization/              # Authorization system
│   │   ├── Handlers/               # Authorization handlers
│   │   ├── Requirements/           # Authorization requirements
│   │   └── Policies/               # Policy definitions
│   ├── Services/                   # External service implementations
│   │   ├── AzureMaps/              # Azure Maps integration
│   │   ├── Notifications/          # Notification services
│   │   └── FileStorage/            # File storage services
│   ├── Logging/                    # Serilog configuration
│   └── Time/                       # NodaTime implementations
├── Presentation/                    # API layer (when endpoints added)
│   ├── Controllers/                # API controllers
│   ├── Middlewares/                # Custom middleware
│   ├── Filters/                    # Action filters
│   └── Models/                     # API models
└── Extensions/                      # DI container extensions
    ├── ServiceCollectionExtensions.cs
    ├── ApplicationBuilderExtensions.cs
    └── ConfigurationExtensions.cs
```

## Key Architectural Decisions

### Identity Integration Strategy
- **Extend Microsoft Identity**: Create `ApplicationUser : IdentityUser<long>` (no separate ProviderId)
- **Claims-Based Authorization**: Permissions become Claims in the Identity system
- **Venue-Specific Security**: Use Role-Venue relationships for row-level security
- **Navigation Properties**: Full EF Core navigation as per Microsoft docs

### Authorization Strategy
- **Permission-Based Claims**: Convert existing Permission system to Claims
- **Venue Context Security**: Role assignments scoped to specific venues
- **Global vs Venue Permissions**: Administrators/Content-Managers = global, others = venue-scoped
- **No Massive Tables**: Use VenueRole relationships instead of permission numbers in claims

### API Design Strategy
- **REST First**: Start with REST endpoints, add GraphQL later
- **Extension Method Setup**: `AddPulseByMirthSystemsEndpoints()` for all API registration

### Infrastructure Decisions
- **Auto-Migration**: Configurable database migration on startup (default: enabled)
- **SignalR**: For real-time ActivityThread updates and notifications
- **Local Storage**: File storage for Phase 1, abstracted for future Azure Blob
- **Azure Maps**: Geocoding/reverse geocoding only (no map rendering)

## Database Design Principles

### Lookup Tables Instead of Enums
Replace all enums with lookup tables:

- `days_of_week` ✓ (already exists)
- `venue_categories` ✓ (already exists)
- `special_categories` ✓ (already exists)
- `notification_types` (new - replace NotificationType enum)
- `media_types` (new - replace MediaType enum)
- `follow_types` (new - replace FollowType enum)

### Identity Integration
Transform existing User entity to extend IdentityUser:

- `ApplicationUser : IdentityUser<long>` (use existing User properties)
- Remove `ProviderId` field (handled by Identity)
- Maintain all existing navigation properties
- Convert to Claims-based authorization

### Entity Relationships
Maintain existing relationships but enhance with:

- Proper indexes for performance
- Cascade delete rules
- Audit trails (created/updated timestamps)
- Soft delete capability where appropriate

## Configuration System

### PulseByMirthSystemsConfiguration

```csharp
public class PulseByMirthSystemsConfiguration
{
    public string ConnectionString { get; set; }
    public string AzureMapsKey { get; set; }
    public SerilogConfiguration Logging { get; set; }
    public IdentityConfiguration Identity { get; set; }
    public AuthorizationConfiguration Authorization { get; set; }
    public NotificationConfiguration Notifications { get; set; }
    public FileStorageConfiguration FileStorage { get; set; }
    public SignalRConfiguration SignalR { get; set; }
    public bool AutoMigrate { get; set; } = true;
    public bool SeedData { get; set; } = true;
}
```

### Extension Methods

```csharp
// Setup core services, Identity, database, logging, etc.
services.AddPulseByMirthSystems(config => {
    config.ConnectionString = connectionString;
    config.AzureMapsKey = azureMapsKey;
    config.AutoMigrate = true;
    config.SeedData = true;
    // ... other configuration
});

// Add REST API endpoints and SignalR hubs
app.AddPulseByMirthSystemsEndpoints();
```

## Service Registration Strategy

### Core Services to Register

1. **IClock** - NodaTime clock for testable time operations
2. **ApplicationDbContext** - Extended IdentityDbContext with our entities
3. **ASP.NET Core Identity** - Extended with ApplicationUser, ApplicationRole
4. **Repository Pattern** - Data access abstraction over EF Core
5. **Domain Services** - Business logic services
6. **Application Services** - Use case orchestration
7. **Azure Maps Services** - Geocoding/reverse geocoding only
8. **SignalR Hubs** - Real-time ActivityThread and notifications
9. **Claims-Based Authorization** - Transform existing RBAC to Claims
10. **Logging Services** - Serilog with PostgreSQL sink

### Auto-Configuration Features

- Database migration on startup (configurable via `AutoMigrate`)
- Seed initial admin user and lookup tables (configurable via `SeedData`)
- Configure Serilog with PostgreSQL sink
- Setup Identity with custom ApplicationUser/ApplicationRole
- Configure Claims-based authorization policies
- Register SignalR hubs for real-time features

## Testing Strategy

### Unit Test Organization
```
PulseByMirthSystems.UnitTests/
├── Domain/                         # Domain logic tests
├── Application/                    # Application service tests
├── Infrastructure/                 # Infrastructure tests
│   ├── Data/                      # Repository tests
│   ├── Services/                  # Service implementation tests
│   └── Authorization/             # Authorization tests
├── Common/                        # Test utilities
│   ├── Fixtures/                  # Test fixtures
│   ├── Builders/                  # Test data builders
│   └── Mocks/                     # Mock implementations
└── Integration/                   # Integration tests
```

### Test Coverage Goals
- **Domain Layer**: 95%+ (pure business logic)
- **Application Layer**: 90%+ (use cases and services)
- **Infrastructure Layer**: 85%+ (external integrations)
- **Overall Target**: 90%+

### Testing Tools
- **XUnit** - Test framework
- **Moq** - Mocking framework
- **FluentAssertions** - Assertion library
- **TestContainers** - Integration testing with real PostgreSQL
- **Bogus** - Test data generation
- **AutoFixture** - Object creation for tests

## Implementation Phases

### Phase 1: Foundation & Identity Integration

1. **Setup Clean Architecture folder structure** in `PulseByMirthSystems` with strict file organization
   - Create folder structure as outlined above
   - Establish naming conventions and file-per-class standards
   - Setup project references and dependencies
2. **Microsoft Identity Integration**:
   - Create `ApplicationUser : IdentityUser<long>` with existing User properties
   - Create `ApplicationRole : IdentityRole<int>` for our roles
   - Create `ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, long>`
   - Transform existing Permission system to Claims
   - Implement navigation properties as per Microsoft documentation
3. Create domain interfaces and value objects (each in separate files)
4. Setup basic DI container extensions with IClock registration
5. Configure EF Core with PostgreSQL and Identity

### Phase 2: Data Layer & Claims Migration

1. **Transform Current Entities to Identity System**:
   - Migrate User → ApplicationUser (extend IdentityUser)
   - Keep existing Role/Permission tables but adapt for Claims
   - Create Claims population service from existing RolePermissions
   - Implement VenueRole for venue-scoped authorization
2. Create repository interfaces and implementations over Identity stores
3. Setup database context with snake_case conventions and Identity tables
4. Implement lookup tables migration (replace enums)
5. Create seed data services with initial admin user via Identity UserManager

### Phase 3: Authorization & Claims System

1. **Claims-Based Authorization Implementation**:
   - Transform existing RBACHandler to use Identity Claims
   - Implement Claims transformation from database permissions
   - Create venue-context extraction for scoped permissions
   - Implement Claims refreshing when roles change
2. Create authorization policies using Claims requirements
3. Setup venue-specific authorization (row-level security via VenueRole)
4. Implement permission caching for performance

### Phase 4: Application Layer & Services

1. Implement application services using Identity UserManager/RoleManager
2. Create feature-based use cases for each domain area
3. Setup mapping between domain entities and DTOs
4. Implement validation using Data Annotations and FluentValidation
5. Add cross-cutting concerns (logging, caching, audit trails)

### Phase 5: Infrastructure Services & Real-time

1. **Azure Maps integration** (geocoding/reverse geocoding only)
2. **SignalR Hubs** for ActivityThreads and real-time notifications  
3. **Local File Storage** service with abstraction for future Azure Blob
4. **Notification services** integrated with SignalR
5. **Serilog configuration** with PostgreSQL sink

### Phase 6: API Layer & Endpoints

1. **REST Controllers** using Identity authentication
2. SignalR hub endpoints for real-time features
3. API documentation with Swagger
4. Rate limiting and security middleware
5. Health checks and monitoring

### Phase 7: Testing & Coverage

1. Unit tests for all layers (target 90%+ coverage)
2. Integration tests with TestContainers PostgreSQL
3. Identity integration tests (login, claims, authorization)
4. SignalR integration tests
5. Performance and load testing

## Key Implementation Notes

### Microsoft Identity Integration Strategy

```csharp
// ApplicationUser extends IdentityUser<long> with our existing properties
public class ApplicationUser : IdentityUser<long>
{
    // Existing User properties (remove ProviderId)
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? DisplayName { get; set; }
    public string? ProfilePicture { get; set; }
    public string? PhoneNumber { get; set; }
    
    // Existing audit fields
    public Instant CreatedAt { get; set; }
    public Instant? UpdatedAt { get; set; }
    public Instant? LastLoginAt { get; set; }
    public bool IsActive { get; set; } = true;
    public Instant? DeactivatedAt { get; set; }
    public long? DeactivatedByUserId { get; set; }
    
    // Navigation properties (enhanced with Claims support)
    public virtual ICollection<IdentityUserClaim<long>> Claims { get; set; }
    public virtual ICollection<IdentityUserRole<long>> UserRoles { get; set; }
    public virtual ICollection<VenueRole> VenueRoles { get; set; }
    // ... other existing navigation properties
}

// ApplicationDbContext extends IdentityDbContext
public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, long>
{
    // Configure both Identity tables AND our custom tables
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder); // Configure Identity tables
        
        // Configure our custom entities
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
```

### IClock Registration

```csharp
services.AddSingleton<IClock>(SystemClock.Instance);
```

### Entity Framework Conventions

- Use snake_case naming convention for all tables (including Identity)
- Implement audit properties (created_at, updated_at)
- Use NodaTime for all temporal data
- Implement soft delete pattern where appropriate
- Use proper indexing strategy for performance

### File Organization Standards

- **One Class Per File**: Each class, interface, enum, record, and model in its own file
- **Namespace Alignment**: File location should match namespace structure
- **Consistent Naming**: 
  - Classes: `PascalCase.cs` (e.g., `ApplicationUser.cs`)
  - Interfaces: `IPascalCase.cs` (e.g., `IUserRepository.cs`)
  - Enums: `PascalCase.cs` (e.g., `UserStatus.cs`)
  - DTOs/Models: `PascalCase.cs` with suffix (e.g., `UserDto.cs`, `CreateUserRequest.cs`)
- **Folder Structure**: Group related files in folders, not in single files
- **Configuration Classes**: Each EF entity configuration in separate file (e.g., `UserConfiguration.cs`)
- **Feature Organization**: Group by feature/domain area, not by technical layer within features

#### Example File Organization:
```
Domain/
├── Entities/
│   ├── ApplicationUser.cs
│   ├── Venue.cs
│   ├── Special.cs
│   ├── ActivityThread.cs
│   └── Post.cs
├── Interfaces/
│   ├── IUserRepository.cs
│   ├── IVenueRepository.cs
│   └── IClaimsService.cs
├── ValueObjects/
│   ├── Address.cs
│   ├── GeolocationPoint.cs
│   └── BusinessHours.cs
└── Events/
    ├── UserRegisteredEvent.cs
    ├── SpecialCreatedEvent.cs
    └── VenueUpdatedEvent.cs

Application/Features/Users/
├── Commands/
│   ├── CreateUserCommand.cs
│   ├── UpdateUserCommand.cs
│   └── DeactivateUserCommand.cs
├── Queries/
│   ├── GetUserByIdQuery.cs
│   ├── GetUsersByVenueQuery.cs
│   └── SearchUsersQuery.cs
├── Models/
│   ├── UserDto.cs
│   ├── CreateUserRequest.cs
│   └── UserSummaryDto.cs
└── Services/
    ├── IUserService.cs
    └── UserService.cs
```

### Claims-Based Authorization System

- Transform existing Permission → Claims pipeline
- Venue-scoped permissions via VenueRole + Claims combination
- Global administrators/content-managers get all Claims
- Venue users get Claims scoped to their assigned venues
- Claims refreshing when role assignments change
- Caching for performance optimization

### SignalR Real-time Features

- ActivityThread updates (new posts, participant joins/leaves)
- Real-time notifications (special alerts, venue updates)
- Venue-scoped broadcasting using Groups
- Authentication integration with Identity system

### Testing Patterns

- Arrange-Act-Assert pattern consistently
- Builder pattern for test data creation
- Dependency injection in tests with mock IClock
- Time-travel testing capabilities with controllable IClock
- Identity integration testing with TestContainers

## Success Criteria

1. **Architecture**: Clean separation of concerns, testable design
2. **Performance**: Sub-200ms response times for API calls  
3. **Security**: Proper Identity authentication and Claims authorization
4. **Testability**: 90%+ code coverage, fast test suite execution
5. **Maintainability**: Clear code structure, comprehensive documentation, one class per file
6. **Organization**: Proper file structure with namespace alignment and consistent naming
7. **Usability**: Simple setup via extension methods, seamless Identity integration

## Next Steps

1. Discuss and refine this plan together
2. Setup the Clean Architecture folder structure
3. Begin Microsoft Identity integration and Claims transformation
4. Implement foundation layer with IClock and basic DI
5. Iteratively build and test each layer with Identity at the core

This plan ensures we build a robust, testable, and maintainable library that seamlessly integrates with ASP.NET Core Identity while maintaining all existing business logic and relationships.
