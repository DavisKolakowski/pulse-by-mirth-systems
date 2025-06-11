# PulseByMirthSystems Library Architecture

## Overview

PulseByMirthSystems is a comprehensive, self-contained library implementing the Pulse social venue application using Clean Architecture principles. The library integrates deeply with ASP.NET Core Identity for authentication and authorization, providing a complete solution for venue-based social networking.

## Core Features

- **Complete Identity Integration**: Extends ASP.NET Core Identity with custom user and role management
- **Claims-Based Authorization**: Venue-scoped and global permissions using Identity Claims
- **Real-Time Communication**: SignalR integration for live activity threads and notifications
- **Geolocation Services**: Azure Maps integration for geocoding and reverse geocoding
- **Comprehensive Testing**: 90%+ test coverage with dependency injection and time-travel testing
- **Clean Architecture**: Domain-driven design with clear separation of concerns

## Architecture Layers

### Domain Layer (`Domain/`)
Pure business logic with no external dependencies:
- **Entities**: Core business entities (ApplicationUser, Venue, Special, etc.)
- **Value Objects**: Immutable objects representing concepts (Address, GeolocationPoint)
- **Interfaces**: Domain service contracts
- **Events**: Domain events for decoupled communication
- **Exceptions**: Domain-specific exceptions

### Application Layer (`Application/`)
Use cases and application logic:
- **Features**: Feature-based organization (Users, Venues, Specials, etc.)
- **Common**: Shared application concerns (interfaces, behaviors, mappings)
- **Services**: Application service implementations

### Infrastructure Layer (`Infrastructure/`)
External concerns and implementations:
- **Data**: Entity Framework Core with PostgreSQL
- **Identity**: ASP.NET Core Identity extensions
- **Authorization**: Claims-based authorization system
- **Services**: External service implementations (Azure Maps, file storage)
- **Logging**: Serilog configuration

### Presentation Layer (`Presentation/`)
API controllers and SignalR hubs (when endpoints are added)

## Microsoft Identity Integration

### ApplicationUser
Extends `IdentityUser<long>` with our business properties:
- Maintains all existing User entity properties
- Removes Auth0 ProviderId dependency
- Integrates with Identity Claims system
- Preserves navigation properties

### ApplicationRole
Extends `IdentityRole<int>` for our role system:
- Maps to existing Role entities
- Integrates with Claims pipeline
- Supports venue-scoped assignments

### ApplicationClaim
Extends `IdentityClaim` for permissions:
- Replaces separate Permission table
- Supports venue-specific claims
- Enables dynamic permission checking

### Claims-Based Authorization
- **Global Claims**: Administrators and Content Managers get all claims
- **Venue-Scoped Claims**: Users get claims only for assigned venues
- **Dynamic Claims**: Claims refreshed when role assignments change
- **Performance Optimization**: Claims caching for frequently accessed permissions

## Database Design

### Identity Tables
Leverages standard ASP.NET Core Identity tables:
- `AspNetUsers` (ApplicationUser extensions)
- `AspNetRoles` (ApplicationRole extensions)
- `AspNetUserClaims` (ApplicationClaim extensions)
- `AspNetUserRoles` (User-Role assignments)
- `AspNetRoleClaims` (Role-based claims)

### Custom Business Tables
Maintains existing business logic:
- All lookup tables (venue_categories, special_categories, etc.)
- Business entities (venues, specials, activity_threads, etc.)
- Relationship tables (venue_roles for scoped permissions)

### Naming Conventions
- **snake_case**: All table and column names
- **Audit Fields**: created_at, updated_at on all entities
- **Soft Delete**: is_active flags where appropriate
- **NodaTime**: All temporal data using Instant/LocalDate

## Real-Time Features

### SignalR Hubs
- **ActivityThreadHub**: Real-time post updates, participant management
- **NotificationHub**: Live notifications for venue updates and specials
- **VenueHub**: Venue-specific broadcasting with group management

### Authentication Integration
- SignalR hubs authenticate using Identity tokens
- Venue-scoped groups based on user permissions
- Automatic connection management

## Configuration & Setup

### Service Registration
```csharp
services.AddPulseByMirthSystems(config => {
    config.ConnectionString = connectionString;
    config.AzureMapsKey = azureMapsKey;
    config.AutoMigrate = true;
    config.SeedData = true;
});
```

### Endpoint Registration
```csharp
app.AddPulseByMirthSystemsEndpoints();
```

### Auto-Configuration Features
- Database migration on startup
- Identity system setup
- Claims population from existing permissions
- Serilog configuration with PostgreSQL sink
- SignalR hub registration

## Security Model

### Authentication
- ASP.NET Core Identity with custom extensions
- JWT token support for API access
- Cookie authentication for web applications
- Automatic token refresh

### Authorization
- Claims-based authorization policies
- Venue-scoped permission checking
- Row-level security via VenueRole relationships
- Permission caching for performance

### Data Protection
- All sensitive data encrypted at rest
- Secure connection requirements
- Input validation and sanitization
- SQL injection protection via EF Core

## Testing Strategy

### Coverage Goals
- **Domain Layer**: 95%+ (pure business logic)
- **Application Layer**: 90%+ (use cases and services)
- **Infrastructure Layer**: 85%+ (external integrations)
- **Overall Target**: 90%+

### Testing Patterns
- **Arrange-Act-Assert**: Consistent test structure
- **Builder Pattern**: Test data creation
- **Dependency Injection**: Injectable dependencies in tests
- **Time-Travel Testing**: Controllable IClock for temporal testing
- **Integration Testing**: TestContainers with real PostgreSQL

### Test Organization
- One test class per production class
- Feature-based test organization
- Separate test projects for unit vs integration tests

## Performance Considerations

### Database Optimization
- Proper indexing on frequently queried columns
- Connection pooling configuration
- Query optimization with EF Core
- Caching strategies for lookup data

### Real-Time Optimization
- SignalR scaling considerations
- Connection management
- Group-based broadcasting
- Message queuing for high volume

### Caching Strategy
- Claims caching for authorization
- Lookup table caching
- User session caching
- Distributed caching support

## Deployment & Operations

### Health Checks
- Database connectivity
- Azure Maps service availability
- SignalR hub health
- Identity service status

### Monitoring & Logging
- Comprehensive Serilog integration
- PostgreSQL log sink
- Performance monitoring
- Error tracking and alerting

### Scaling Considerations
- Stateless design for horizontal scaling
- SignalR backplane configuration
- Database connection scaling
- Caching layer scaling

## Migration from Existing System

### Data Migration
- User data transformation to ApplicationUser
- Permission to Claims conversion
- Existing relationships preservation
- Incremental migration support

### Compatibility
- Maintains all existing business logic
- Preserves data relationships
- Backwards compatible API design
- Gradual adoption support

## Development Guidelines

### File Organization
- One class per file
- Namespace alignment with folder structure
- Consistent naming conventions
- Feature-based organization

### Code Quality
- Comprehensive XML documentation
- Consistent error handling
- Dependency injection patterns
- SOLID principles adherence

### Version Control
- Clear commit messages
- Feature branch workflow
- Automated testing on commits
- Code review requirements

This architecture ensures a robust, scalable, and maintainable solution that integrates seamlessly with ASP.NET Core Identity while preserving all existing business logic and relationships.
