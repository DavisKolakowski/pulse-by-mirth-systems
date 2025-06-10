# Pulse Database-Driven Authorization System

## Overview

Pulse uses a **hybrid authentication and authorization approach** that separates concerns between external identity providers and internal business logic:

- **Authentication**: Handled by Auth0 (identity verification, JWT tokens)
- **Authorization**: Handled entirely by our PostgreSQL database (permissions, roles, venue access)

This approach provides maximum flexibility, performance, and control over our authorization logic while leveraging Auth0's robust authentication capabilities.

## Architecture Components

### 1. Authentication Layer (Auth0)
- **Purpose**: Identity verification and JWT token issuance
- **Responsibilities**:
  - User authentication (login/logout)
  - JWT token generation and validation
  - Social login integrations
  - Multi-factor authentication
  - Password policies and security

### 2. Authorization Layer (Database)
- **Purpose**: Business logic and access control
- **Responsibilities**:
  - Permission definitions and management
  - Role-based access control (RBAC)
  - Venue-specific role assignments
  - Dynamic permission checking
  - Audit trail for role changes

## Database Schema

### Core Authorization Entities

#### Users Table
```sql
CREATE TABLE users (
    id BIGSERIAL PRIMARY KEY,
    provider_id VARCHAR(100) NOT NULL UNIQUE,  -- Auth0 user ID
    email VARCHAR(100) NOT NULL,
    first_name VARCHAR(50),
    last_name VARCHAR(50),
    display_name VARCHAR(100),
    profile_picture VARCHAR(500),
    phone_number VARCHAR(20),
    created_at TIMESTAMPTZ NOT NULL,
    updated_at TIMESTAMPTZ,
    last_login_at TIMESTAMPTZ,
    is_active BOOLEAN NOT NULL DEFAULT true,
    deactivated_at TIMESTAMPTZ,
    deactivated_by_user_id BIGINT
);
```

#### Roles Table
```sql
CREATE TABLE roles (
    id SERIAL PRIMARY KEY,
    name VARCHAR(50) NOT NULL UNIQUE,
    display_name VARCHAR(100) NOT NULL,
    description VARCHAR(500),
    is_active BOOLEAN NOT NULL DEFAULT true,
    sort_order INTEGER NOT NULL,
    created_at TIMESTAMPTZ NOT NULL
);
```

#### Permissions Table
```sql
CREATE TABLE permissions (
    id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL UNIQUE,  -- e.g., "read:venues", "write:specials"
    display_name VARCHAR(100) NOT NULL,
    description VARCHAR(500),
    category VARCHAR(50) NOT NULL,      -- "Venue", "Special", "Content", etc.
    action VARCHAR(20) NOT NULL,        -- "read", "write", "delete", "moderate"
    resource VARCHAR(50) NOT NULL,      -- "venues", "specials", "content"
    is_active BOOLEAN NOT NULL DEFAULT true,
    sort_order INTEGER NOT NULL,
    created_at TIMESTAMPTZ NOT NULL
);
```

#### Role-Permission Mapping
```sql
CREATE TABLE role_permissions (
    role_id INTEGER NOT NULL REFERENCES roles(id) ON DELETE CASCADE,
    permission_id INTEGER NOT NULL REFERENCES permissions(id) ON DELETE CASCADE,
    PRIMARY KEY (role_id, permission_id)
);
```

#### User Global Roles
```sql
CREATE TABLE user_roles (
    id BIGSERIAL PRIMARY KEY,
    user_id BIGINT NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    role_id INTEGER NOT NULL REFERENCES roles(id) ON DELETE CASCADE,
    assigned_at TIMESTAMPTZ NOT NULL,
    assigned_by_user_id BIGINT REFERENCES users(id) ON DELETE SET NULL,
    is_active BOOLEAN NOT NULL DEFAULT true,
    deactivated_at TIMESTAMPTZ,
    deactivated_by_user_id BIGINT REFERENCES users(id) ON DELETE SET NULL,
    UNIQUE(user_id, role_id) WHERE is_active = true
);
```

#### User Venue-Specific Roles
```sql
CREATE TABLE venue_roles (
    id BIGSERIAL PRIMARY KEY,
    user_id BIGINT NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    venue_id BIGINT NOT NULL REFERENCES venues(id) ON DELETE CASCADE,
    role_id INTEGER NOT NULL REFERENCES roles(id) ON DELETE RESTRICT,
    assigned_at TIMESTAMPTZ NOT NULL,
    assigned_by_user_id BIGINT REFERENCES users(id) ON DELETE SET NULL,
    is_active BOOLEAN NOT NULL DEFAULT true,
    deactivated_at TIMESTAMPTZ,
    deactivated_by_user_id BIGINT REFERENCES users(id) ON DELETE SET NULL,
    UNIQUE(user_id, venue_id, role_id) WHERE is_active = true
);
```

## Authorization Roles

### Global Roles

#### 1. Administrator
- **Scope**: Platform-wide
- **Permissions**: All permissions across the entire system
- **Use Cases**: System administration, global configuration, emergency access
- **Example Users**: Platform owners, lead developers

#### 2. Content Manager
- **Scope**: Platform-wide content
- **Permissions**: 
  - All venue read/write operations
  - Content moderation across all venues
  - Tag and category management
  - Analytics access (platform-wide)
- **Use Cases**: Content curation, platform moderation, data analysis
- **Example Users**: Community managers, data analysts

### Venue-Specific Roles

#### 3. Venue Owner
- **Scope**: Specific venue(s)
- **Permissions**:
  - Full venue management (read/write/delete)
  - User management for their venue(s)
  - All content operations within their venue(s)
  - Analytics for their venue(s)
- **Use Cases**: Business owners, franchise managers
- **Example Users**: Bar owners, restaurant managers

#### 4. Venue Manager
- **Scope**: Specific venue(s)
- **Permissions**:
  - Venue content management (read/write)
  - Special and event management
  - Media uploads
  - Basic analytics for their venue(s)
- **Use Cases**: Day-to-day operations, marketing staff
- **Example Users**: Marketing coordinators, event managers

## Permission System

### Permission Naming Convention
Permissions follow the pattern: `{action}:{resource}`

**Actions**:
- `read` - View/query data
- `write` - Create/update data
- `delete` - Remove data
- `moderate` - Content moderation actions
- `upload` - File/media uploads
- `manage` - Complex operations (follows, notifications)
- `admin` - Administrative operations
- `config` - System configuration

**Resources**:
- `venues` - Venue information and settings
- `specials` - Special offers and events
- `content` - User-generated content
- `posts` - Activity thread posts
- `media` - Photos and videos
- `tags` - Content tags
- `vibes` - Venue atmosphere tags
- `venue-categories` - Venue classification
- `analytics` - Performance metrics
- `notifications` - System notifications
- `follows` - User follows/subscriptions
- `venue-users` - Venue staff assignments
- `system` - System-wide operations

### Sample Permissions

```csharp
// Venue Management
"read:venues"           // Query venue information
"write:venues"          // Create/update venues
"delete:venues"         // Remove venues

// Content Management
"read:content"          // View all content
"write:content"         // Create/edit content
"delete:content"        // Remove content
"moderate:content"      // Content moderation actions

// Special Management
"write:specials"        // Create/edit special offers
"delete:specials"       // Remove specials

// Media Management
"upload:media"          // Upload photos/videos
"delete:media"          // Remove media files
"moderate:media"        // Media content moderation

// Analytics
"read:analytics"        // Venue-specific analytics
"read:analytics-global" // Platform-wide analytics

// System Administration
"admin:system"          // Full system administration
"config:system"         // System configuration
```

## Implementation Strategy

### Phase 1: Database Setup ✅ (COMPLETED)
- [x] Create all authorization tables
- [x] Seed initial roles and permissions
- [x] Add Davis Kolakowski as Administrator
- [x] Implement EF Core configurations
- [x] Generate and test initial migration

### Phase 2: Authorization Middleware (NEXT)
1. **Create Database Authorization Service**
   ```csharp
   public interface IAuthorizationService
   {
       Task<bool> HasPermissionAsync(string userId, string permission, long? venueId = null);
       Task<IEnumerable<string>> GetUserPermissionsAsync(string userId, long? venueId = null);
       Task<IEnumerable<VenueRole>> GetUserVenueRolesAsync(string userId);
       Task<User?> GetUserByProviderIdAsync(string providerId);
   }
   ```

2. **Update Authorization Handlers**
   - Modify `RBACHandler` to use database instead of Auth0 permissions
   - Remove Auth0 Management API dependencies
   - Add venue-specific permission checking

3. **Create Permission Policy Provider**
   ```csharp
   public class DatabasePermissionPolicyProvider : IAuthorizationPolicyProvider
   {
       // Auto-create policies for any permission found in database
       // No more manual policy registration needed
   }
   ```

### Phase 3: Controller Updates
1. **Remove Auth0 Permission Attributes**
   ```csharp
   // OLD: Auth0 management
   [Authorize(Policy = "read:venues")]
   
   // NEW: Database-driven (same syntax, different implementation)
   [Authorize(Policy = "read:venues")]
   ```

2. **Add Venue Context Support**
   ```csharp
   [Authorize(Policy = "write:specials")]
   [Route("venues/{venueId}/specials")]
   public async Task<IActionResult> CreateSpecial(long venueId, CreateSpecialRequest request)
   {
       // Authorization handler automatically checks venue-specific permissions
   }
   ```

### Phase 4: User Management API
1. **Create User Management Endpoints**
   - `POST /api/users/{userId}/roles` - Assign global role
   - `DELETE /api/users/{userId}/roles/{roleId}` - Remove global role
   - `POST /api/venues/{venueId}/users/{userId}/roles` - Assign venue role
   - `DELETE /api/venues/{venueId}/users/{userId}/roles/{roleId}` - Remove venue role

2. **Create Permission Query Endpoints**
   - `GET /api/users/me/permissions` - Current user's permissions
   - `GET /api/users/me/venues` - Venues user has access to
   - `GET /api/venues/{venueId}/permissions` - Check venue-specific access

### Phase 5: Frontend Integration
1. **Update React Auth Context**
   - Remove Auth0 permission checking
   - Add database permission API calls
   - Implement permission-based UI rendering

2. **Create Admin UI**
   - User role management interface
   - Venue role assignment interface
   - Permission audit trail

## API Usage Examples

### Authentication Flow
```typescript
// 1. User authenticates with Auth0
const token = await auth0.getAccessTokenSilently();

// 2. Frontend sends Auth0 JWT to API
const response = await fetch('/api/venues', {
  headers: { 'Authorization': `Bearer ${token}` }
});

// 3. API validates JWT and extracts Auth0 user ID
// 4. API looks up user in database by provider_id
// 5. API checks database permissions for the operation
// 6. API returns data based on user's database permissions
```

### Permission Checking in Controllers
```csharp
[ApiController]
[Route("api/[controller]")]
public class VenuesController : ControllerBase
{
    [HttpGet]
    [Authorize(Policy = "read:venues")]
    public async Task<IActionResult> GetVenues()
    {
        // User automatically has read:venues permission
        // Either globally or for specific venues they manage
        return Ok(venues);
    }

    [HttpPost]
    [Route("{venueId}/specials")]
    [Authorize(Policy = "write:specials")]
    public async Task<IActionResult> CreateSpecial(long venueId, CreateSpecialRequest request)
    {
        // Authorization handler checks:
        // 1. Does user have global write:specials permission? OR
        // 2. Does user have write:specials for this specific venue?
        return Ok(special);
    }
}
```

### Venue-Specific Authorization Logic
```csharp
public class DatabaseAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        var user = await GetCurrentUserAsync(context);
        var venueId = GetVenueIdFromRequest(context);
        
        // Check global permissions first
        var hasGlobalPermission = await HasGlobalPermissionAsync(user.Id, requirement.Permission);
        if (hasGlobalPermission)
        {
            context.Succeed(requirement);
            return;
        }
        
        // Check venue-specific permissions if venue context exists
        if (venueId.HasValue)
        {
            var hasVenuePermission = await HasVenuePermissionAsync(user.Id, requirement.Permission, venueId.Value);
            if (hasVenuePermission)
            {
                context.Succeed(requirement);
                return;
            }
        }
        
        context.Fail();
    }
}
```

## Benefits of This Approach

### 1. **Performance**
- No external API calls during authorization
- Database queries are fast and cacheable
- Venue-specific permissions resolved in single query

### 2. **Flexibility**
- Easy to add new permissions without Auth0 configuration
- Complex business logic in familiar SQL/C#
- Venue-specific roles without Auth0 limitations

### 3. **Auditability**
- Complete audit trail in database
- Track who assigned/removed roles and when
- Historical permission data for compliance

### 4. **Scalability**
- No Auth0 Management API rate limits
- Database can handle high-volume permission checks
- Easy to add caching layers

### 5. **Maintainability**
- Single source of truth for authorization
- No synchronization between Auth0 and database
- Standard EF Core patterns for data access

## Migration Strategy

### From Auth0 Permissions to Database
1. **Keep existing Auth0 authentication** - No changes to login flow
2. **Remove Auth0 Management API** - Delete permission sync code
3. **Update authorization handlers** - Query database instead of JWT claims
4. **Preserve existing controller attributes** - Same `[Authorize(Policy = "permission")]` syntax
5. **Add venue context support** - Automatic venue-specific permission checking

### Rollback Plan
- Database schema includes `is_active` flags for all entities
- Can temporarily disable database authorization by feature flag
- Original Auth0 permission checking code preserved in git history

## Security Considerations

### 1. **Token Validation**
- Continue validating Auth0 JWT signatures
- Verify token expiration and audience
- Extract `sub` claim for user identification

### 2. **Database Security**
- Use parameterized queries (EF Core handles this)
- Implement proper connection string encryption
- Regular database backups and monitoring

### 3. **Permission Caching**
- Cache user permissions for request duration
- Invalidate cache on role changes
- Consider Redis for multi-instance deployments

### 4. **Audit Logging**
- Log all permission checks and results
- Track role assignment/removal operations
- Monitor for unusual permission patterns

## Testing Strategy

### Unit Tests
- Test permission checking logic with various role combinations
- Test venue-specific permission inheritance
- Mock database context for fast test execution

### Integration Tests
- Test complete authentication + authorization flow
- Verify JWT validation + database permission checking
- Test edge cases (inactive users, deleted venues, etc.)

### Load Tests
- Benchmark permission checking performance
- Test with realistic user/venue/permission volumes
- Identify caching opportunities

## Future Enhancements

### 1. **Dynamic Permissions**
- Create permissions through admin UI
- Runtime permission validation
- A/B testing for new permissions

### 2. **Permission Groups**
- Logical grouping of related permissions
- Easier role management
- Hierarchical permission inheritance

### 3. **Temporary Access**
- Time-limited role assignments
- Event-specific permissions
- Automatic role expiration

### 4. **Advanced Analytics**
- Permission usage analytics
- Role effectiveness metrics
- Security anomaly detection

---

This database-driven authorization system provides the foundation for scalable, maintainable, and flexible access control while preserving the developer experience and security benefits of our current Auth0 integration.
