# Pulse Database-Only Authorization System

> **IMPORTANT UPDATE**: As of our latest implementation, Pulse has migrated to a **database-only authorization system**. Auth0 is now used solely for authentication, while all authorization logic is handled by our PostgreSQL database.

## System Overview

### Authentication (Auth0) ✅
- **Purpose**: Identity verification only
- **Responsibilities**: Login, JWT tokens, user identity
- **What Auth0 Provides**: `sub` claim (user identifier)

### Authorization (Database) ✅
- **Purpose**: All permission checking and role management
- **Responsibilities**: Roles, permissions, venue-specific access
- **What Database Provides**: Complete access control logic

## Key Architecture Changes

### Before (Hybrid)
```
Auth0 JWT Token → Contains permissions → API checks JWT claims
Database → Contains venue relationships → API checks venue access
```

### After (Database-Only) ✅
```
Auth0 JWT Token → Contains only user ID → API looks up user in database
Database → Contains ALL authorization logic → API checks database permissions
```

## Database Schema

### Core Tables

```sql
-- Users (linked to Auth0 by provider_id)
users (
    id BIGSERIAL PRIMARY KEY,
    provider_id VARCHAR(100) UNIQUE, -- Auth0 'sub' claim
    email VARCHAR(100),
    ...
)

-- Roles (Administrator, Content Manager, Venue Owner, Venue Manager)
roles (
    id SERIAL PRIMARY KEY,
    name VARCHAR(50) UNIQUE,
    display_name VARCHAR(100),
    ...
)

-- Permissions (read:venues, write:specials, etc.)
permissions (
    id SERIAL PRIMARY KEY,
    name VARCHAR(100) UNIQUE,
    display_name VARCHAR(100),
    category VARCHAR(50),  -- Venue, Special, Content, etc.
    action VARCHAR(20),    -- read, write, delete, moderate
    resource VARCHAR(50),  -- venues, specials, content
    ...
)

-- Role-Permission mapping
role_permissions (
    role_id INTEGER REFERENCES roles(id),
    permission_id INTEGER REFERENCES permissions(id),
    PRIMARY KEY (role_id, permission_id)
)

-- Global role assignments  
user_roles (
    id BIGSERIAL PRIMARY KEY,
    user_id BIGINT REFERENCES users(id),
    role_id INTEGER REFERENCES roles(id),
    is_active BOOLEAN DEFAULT true,
    assigned_at TIMESTAMPTZ,
    ...
)

-- Venue-specific role assignments
venue_roles (
    id BIGSERIAL PRIMARY KEY,
    user_id BIGINT REFERENCES users(id),
    venue_id BIGINT REFERENCES venues(id), 
    role_id INTEGER REFERENCES roles(id),
    is_active BOOLEAN DEFAULT true,
    assigned_at TIMESTAMPTZ,
    ...
)
```

## Authorization Flow

### 1. User Authentication
```
User → Auth0 Login → JWT Token (contains 'sub' claim only)
```

### 2. API Request
```
Frontend → API Request with JWT → API validates JWT → Extracts Auth0 user ID
```

### 3. Database Authorization
```csharp
// 1. Look up user by Auth0 ID
var user = await GetUserByProviderIdAsync(auth0UserId);

// 2. Check global permissions
var hasGlobalPermission = await HasGlobalPermissionAsync(user.Id, "read:venues");

// 3. Check venue-specific permissions (if applicable)
var hasVenuePermission = await HasVenuePermissionAsync(user.Id, "write:specials", venueId);

// 4. Grant or deny access
if (hasGlobalPermission || hasVenuePermission) {
    // Allow access
} else {
    // Deny access (403)
}
```

## Roles and Permissions

### Seeded Roles

#### 1. Administrator (Global)
```sql
INSERT INTO user_roles (user_id, role_id) VALUES (1, 1); -- Davis = Administrator
```
- **Scope**: Platform-wide
- **Permissions**: All 35 permissions
- **Examples**: System administration, global analytics, user management

#### 2. Content Manager (Global)
- **Scope**: Platform-wide content
- **Permissions**: Content moderation, venue management, analytics
- **Examples**: Community management, content curation

#### 3. Venue Owner (Venue-Specific)
```sql
INSERT INTO venue_roles (user_id, venue_id, role_id) VALUES (2, 1, 3); -- User 2 owns Venue 1
```
- **Scope**: Specific venues only
- **Permissions**: Full venue management for assigned venues
- **Examples**: Business owners, franchise managers

#### 4. Venue Manager (Venue-Specific)
```sql
INSERT INTO venue_roles (user_id, venue_id, role_id) VALUES (3, 1, 4); -- User 3 manages Venue 1
```
- **Scope**: Specific venues only  
- **Permissions**: Content and special management for assigned venues
- **Examples**: Marketing staff, event coordinators

### Sample Permissions (35 total)
```
Venue Management:
- read:venues, write:venues, delete:venues

Special Management:  
- write:specials, delete:specials

Content Management:
- read:content, write:content, delete:content, moderate:content

Media Management:
- upload:media, delete:media, moderate:media

Analytics:
- read:analytics, read:analytics-global

System Administration:
- admin:system, config:system

... and 20 more
```

## Controller Implementation

### Authorization Attributes (Unchanged)
```csharp
[Authorize(Policy = "read:venues")]
public async Task<IActionResult> GetVenues()
{
    // Database automatically checks if user has read:venues permission
    // Either globally or for specific venues they manage
}

[Authorize(Policy = "write:specials")]
[Route("venues/{venueId}/specials")]
public async Task<IActionResult> CreateSpecial(long venueId, CreateSpecialRequest request)
{
    // Database checks if user has write:specials permission for this venue
    // OR has global write:specials permission
}
```

### Authorization Service
```csharp
public interface IAuthorizationService
{
    Task<User?> GetUserByProviderIdAsync(string providerId);
    Task<bool> HasPermissionAsync(long userId, string permission, long? venueId = null);
    Task<IEnumerable<string>> GetUserPermissionsAsync(long userId, long? venueId = null);
    Task<bool> IsUserActiveAsync(long userId);
}
```

## Implementation Status ✅

### Completed
- [x] Database schema with all tables
- [x] EF Core entities and configurations
- [x] Initial migration with seeded data
- [x] Snake case naming for PostgreSQL
- [x] Davis Kolakowski as Administrator
- [x] PostGIS support for venue locations
- [x] Aspire integration working

### Next Steps (Tomorrow)
- [ ] Remove Auth0 Management API dependencies
- [ ] Implement `DatabaseAuthorizationService`
- [ ] Update `RBACHandler` to use database
- [ ] Test complete authentication + authorization flow
- [ ] Update frontend to use database permissions

## Benefits of Database-Only Approach

### 1. **Performance**
- No external API calls during authorization
- Single database query for permission checking
- Easy to cache user permissions

### 2. **Flexibility** 
- Add new permissions without Auth0 configuration
- Complex venue-specific business logic in SQL
- Easy to audit and modify permissions

### 3. **Cost Savings**
- No Auth0 Management API usage fees
- No per-user permission storage costs
- Reduced Auth0 plan requirements

### 4. **Simplicity**
- Single source of truth (database)
- No synchronization between Auth0 and database
- Standard EF Core patterns

### 5. **Venue Context**
- Native support for venue-specific roles
- Automatic venue permission scoping
- Easy venue staff management

## Testing Strategy

### Database Tests
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
    // Venue Manager should have permission for their assigned venue
    var hasPermission = await _authService.HasPermissionAsync(2, "write:specials", venueId: 1);
    Assert.IsTrue(hasPermission);
}
```

### Integration Tests
```csharp
[Test]
public async Task GetVenues_WithValidToken_ReturnsVenues()
{
    // Test complete flow: JWT validation → User lookup → Permission check
    var response = await _client.GetAsync("/api/venues", headers: { Authorization = "Bearer " + validJwt });
    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
}
```

## Security Considerations

### JWT Validation
- Continue validating Auth0 JWT signatures
- Extract only `sub` claim for user identification
- Validate token expiration and audience

### Database Security
- Use parameterized queries (EF Core handles this)
- Implement connection string encryption
- Regular permission audits

### Performance Optimization
- Cache user permissions per request
- Index database tables appropriately
- Monitor query performance

---

**Summary**: Pulse now uses Auth0 solely for authentication (login, JWT tokens) while handling all authorization through our PostgreSQL database. This provides better performance, flexibility, and cost-effectiveness while maintaining security and enabling complex venue-specific access control.
