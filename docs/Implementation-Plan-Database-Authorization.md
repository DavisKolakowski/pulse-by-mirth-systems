# Implementation Plan: Database-Driven Authorization

## Current Status ✅

### Completed Today
- [x] **Database Schema**: Complete EF Core entities and configurations
- [x] **Migration**: Successfully created and tested `InitialCreate` migration
- [x] **Seeded Data**: All roles, permissions, and initial admin user (Davis)
- [x] **Snake Case Naming**: PostgreSQL-friendly column naming
- [x] **PostGIS Support**: Geographic data support for venue locations
- [x] **Aspire Integration**: Local development environment working

### Verified Working
- [x] Database creates successfully with all tables
- [x] Snake case naming convention applied (`user_roles`, `venue_categories`, etc.)
- [x] All foreign key relationships established
- [x] Initial seed data populated (4 roles, 35 permissions, admin user)
- [x] Aspire dashboard accessible at `https://localhost:17190`

## Tomorrow's Implementation Plan

### Phase 1: Remove Auth0 Authorization Dependencies (2-3 hours)

#### 1.1 Update NuGet Packages
```xml
<!-- REMOVE from Pulse.Core.csproj -->
<PackageReference Include="Auth0.ManagementApi" Version="x.x.x" />

<!-- KEEP in Pulse.API.csproj -->
<PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" Version="x.x.x" />
```

#### 1.2 Delete Auth0 Management Code
**Files to Remove:**
- `src/Pulse.Core/Services/Auth0ManagementService.cs` (if exists)
- `src/Pulse.Core/Configuration/Auth0ManagementConfig.cs` (if exists)
- Any Auth0 Management API client registration in DI

**Files to Update:**
- `src/Pulse.API/Program.cs` - Remove Auth0 Management API configuration
- `src/Pulse.Core/Extensions/ServiceCollectionExtensions.cs` - Remove Auth0 service registrations

#### 1.3 Clean Up Configuration
**Remove from appsettings.json:**
```json
{
  "Auth0Management": {
    "Domain": "...",
    "ClientId": "...",
    "ClientSecret": "..."
  }
}
```

**Keep for authentication:**
```json
{
  "Auth0": {
    "Domain": "dev-pulse-app.us.auth0.com",
    "Audience": "pulse-api"
  }
}
```

### Phase 2: Implement Database Authorization Service (3-4 hours)

#### 2.1 Create Authorization Service Interface
**File:** `src/Pulse.Core/Services/IAuthorizationService.cs`
```csharp
public interface IAuthorizationService
{
    Task<User?> GetUserByProviderIdAsync(string providerId);
    Task<bool> HasPermissionAsync(long userId, string permission, long? venueId = null);
    Task<IEnumerable<string>> GetUserPermissionsAsync(long userId, long? venueId = null);
    Task<IEnumerable<VenueRole>> GetUserVenueRolesAsync(long userId);
    Task<bool> IsUserActiveAsync(long userId);
}
```

#### 2.2 Implement Database Authorization Service
**File:** `src/Pulse.Core/Services/DatabaseAuthorizationService.cs`
```csharp
public class DatabaseAuthorizationService : IAuthorizationService
{
    private readonly ApplicationDbContext _context;
    private readonly IMemoryCache _cache;
    
    // Implementation with caching for performance
    // Global permission checking
    // Venue-specific permission checking
    // User lookup by Auth0 provider ID
}
```

#### 2.3 Update Dependency Injection
**File:** `src/Pulse.Core/Extensions/ServiceCollectionExtensions.cs`
```csharp
public static IServiceCollection AddCoreServices(this IServiceCollection services)
{
    services.AddScoped<IAuthorizationService, DatabaseAuthorizationService>();
    services.AddMemoryCache(); // For permission caching
    return services;
}
```

### Phase 3: Update Authorization Handlers (2-3 hours)

#### 3.1 Modify RBACHandler
**File:** `src/Pulse.Core/Authorization/RBACHandler.cs`

**Current Logic (Auth0-based):**
```csharp
// Check JWT claims for permissions
var permissions = context.User.FindAll("permissions").Select(c => c.Value);
if (permissions.Contains(requirement.Permission))
    context.Succeed(requirement);
```

**New Logic (Database-based):**
```csharp
// Extract Auth0 user ID from JWT
var auth0UserId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
if (string.IsNullOrEmpty(auth0UserId)) return;

// Get user from database
var user = await _authorizationService.GetUserByProviderIdAsync(auth0UserId);
if (user == null || !await _authorizationService.IsUserActiveAsync(user.Id)) return;

// Extract venue ID from route/context
var venueId = GetVenueIdFromContext(context);

// Check permission in database
if (await _authorizationService.HasPermissionAsync(user.Id, requirement.Permission, venueId))
    context.Succeed(requirement);
```

#### 3.2 Add Venue Context Extraction
**Helper method in RBACHandler:**
```csharp
private long? GetVenueIdFromContext(AuthorizationHandlerContext context)
{
    // Extract venueId from route data, query string, or request body
    if (context.Resource is HttpContext httpContext)
    {
        // Check route values first: /api/venues/{venueId}/specials
        if (httpContext.Request.RouteValues.TryGetValue("venueId", out var venueIdObj))
            return long.TryParse(venueIdObj?.ToString(), out var venueId) ? venueId : null;
            
        // Check query string: ?venueId=123
        if (httpContext.Request.Query.TryGetValue("venueId", out var venueIdQuery))
            return long.TryParse(venueIdQuery.FirstOrDefault(), out var venueId) ? venueId : null;
    }
    return null;
}
```

#### 3.3 Update Policy Provider (Optional Enhancement)
**File:** `src/Pulse.Core/Authorization/DatabasePermissionPolicyProvider.cs`
```csharp
public class DatabasePermissionPolicyProvider : IAuthorizationPolicyProvider
{
    // Auto-create policies for any permission name
    // No more manual policy registration needed
    // Fallback to default policy provider for other policies
}
```

### Phase 4: Test and Validate (1-2 hours)

#### 4.1 Create Authorization Tests
**File:** `tests/Pulse.Core.Tests/Services/DatabaseAuthorizationServiceTests.cs`
```csharp
[Test]
public async Task HasPermissionAsync_GlobalAdmin_ReturnsTrue()
{
    // Test global administrator permissions
}

[Test] 
public async Task HasPermissionAsync_VenueSpecific_ReturnsTrue()
{
    // Test venue-specific permissions
}

[Test]
public async Task HasPermissionAsync_NoPermission_ReturnsFalse()
{
    // Test permission denial
}
```

#### 4.2 Integration Testing
1. **Start Aspire Host**: Verify database connection
2. **Test Endpoints**: Use existing controllers with new authorization
3. **Check Logs**: Verify no Auth0 Management API calls
4. **Performance Test**: Measure permission check speed

#### 4.3 Postman/API Testing
```http
# Test with Davis's Auth0 token
GET https://localhost:7125/api/venues
Authorization: Bearer {auth0_jwt_token}

# Should work - Davis has Administrator role with read:venues permission
```

### Phase 5: Frontend Updates (1-2 hours)

#### 5.1 Remove Auth0 Permission Checking
**File:** `src/Pulse.Clients.Web/src/auth0-provider-with-navigate.tsx`
```typescript
// REMOVE: useAuth0 permission checking
const { user, isAuthenticated, getAccessTokenSilently } = useAuth0();

// KEEP: Authentication state and token retrieval
```

#### 5.2 Create Permission API Service
**File:** `src/Pulse.Clients.Web/src/services/permissionService.ts`
```typescript
export class PermissionService {
  async getUserPermissions(): Promise<string[]> {
    const token = await getAccessTokenSilently();
    const response = await fetch('/api/users/me/permissions', {
      headers: { 'Authorization': `Bearer ${token}` }
    });
    return response.json();
  }
  
  async hasPermission(permission: string, venueId?: number): Promise<boolean> {
    // Call API to check specific permission
  }
}
```

#### 5.3 Update Permission Guards
**File:** `src/Pulse.Clients.Web/src/components/PermissionGuard.tsx`
```typescript
// Change from Auth0 permission checking to API-based checking
```

## Testing Checklist

### ✅ Database Verification
- [ ] All tables created with correct schema
- [ ] Snake case naming applied consistently  
- [ ] Foreign key relationships working
- [ ] Seed data populated correctly
- [ ] Davis has Administrator role assigned

### ✅ Authentication Flow
- [ ] Auth0 login still works
- [ ] JWT tokens validated correctly
- [ ] User lookup by provider_id works
- [ ] Inactive users properly blocked

### ✅ Authorization Flow  
- [ ] Global permissions work (Administrator role)
- [ ] Venue-specific permissions work
- [ ] Permission denial works correctly
- [ ] Caching improves performance

### ✅ API Endpoints
- [ ] GET /api/venues (requires read:venues)
- [ ] POST /api/venues (requires write:venues)
- [ ] GET /api/venues/{id}/specials (venue-specific)
- [ ] POST /api/venues/{id}/specials (venue-specific write)

### ✅ Error Handling
- [ ] Invalid tokens return 401
- [ ] Insufficient permissions return 403
- [ ] Database errors handled gracefully
- [ ] Logging captures authorization events

## Performance Considerations

### Caching Strategy
1. **Permission Cache**: Cache user permissions for request duration
2. **User Cache**: Cache user lookup for 5 minutes
3. **Role Cache**: Cache role assignments for 10 minutes

### Database Optimization
1. **Indexes**: Ensure proper indexes on lookup columns
2. **Query Optimization**: Use efficient joins for permission checking
3. **Connection Pooling**: Configure appropriate pool sizes

### Monitoring
1. **Response Times**: Track authorization check performance
2. **Cache Hit Rates**: Monitor caching effectiveness
3. **Database Load**: Watch for authorization query impact

## Rollback Plan

If issues arise during implementation:

1. **Feature Flag**: Add database authorization toggle
2. **Fallback Code**: Keep Auth0 permission logic commented
3. **Database Backup**: Ensure clean state can be restored
4. **Monitoring**: Watch error rates and performance metrics

## Definition of Done

### Phase 1 Complete When:
- [ ] No Auth0 Management API dependencies remain
- [ ] Application builds without Auth0 Management packages
- [ ] Configuration cleaned up

### Phase 2 Complete When:
- [ ] Database authorization service implemented
- [ ] User lookup by provider_id works
- [ ] Permission checking logic complete
- [ ] Caching implemented

### Phase 3 Complete When:
- [ ] RBACHandler uses database instead of JWT claims
- [ ] Venue context extraction working
- [ ] All existing controllers work unchanged

### Phase 4 Complete When:
- [ ] Unit tests pass
- [ ] Integration tests verify end-to-end flow
- [ ] Performance meets requirements (< 50ms for permission checks)

### Phase 5 Complete When:
- [ ] Frontend uses API for permission checking
- [ ] No Auth0 permission dependencies in frontend
- [ ] UI correctly shows/hides based on database permissions

## Success Metrics

### Functional
- ✅ All existing API endpoints work unchanged
- ✅ Davis can access admin functionality
- ✅ Venue-specific permissions properly scoped
- ✅ Permission denial works correctly

### Performance
- ✅ Authorization checks complete in < 50ms
- ✅ Database queries optimized with proper indexes
- ✅ Caching reduces database load by > 80%

### Maintainability
- ✅ Clean separation between authentication and authorization
- ✅ Easy to add new permissions without code changes
- ✅ Clear audit trail for all permission changes
- ✅ Comprehensive test coverage

---

**Estimated Total Time: 8-12 hours**
**Target Completion: End of next business day**
**Risk Level: Medium** (well-defined requirements, existing working database)

The implementation preserves all existing controller code while moving authorization logic to the database, providing a smooth transition with immediate benefits in flexibility and performance.

---

## Summary: Ready for Database-Only Authorization

### Today's Major Accomplishments ✅

1. **Complete Database Schema**: All entities, relationships, and configurations implemented
2. **Working Migration**: Database creates successfully with proper snake_case naming  
3. **Seeded Data**: Davis Kolakowski as Administrator with all permissions
4. **PostGIS Integration**: Geographic support for venue locations
5. **Aspire Orchestration**: Local development environment fully functional
6. **Documentation Cleanup**: Simplified Auth0 to authentication-only

### Tomorrow's Focus 🎯

**Single Goal**: Implement `DatabaseAuthorizationService` and update `RBACHandler` to use database instead of Auth0 JWT claims.

**Key Changes Needed**:

1. `RBACHandler.cs` - Change from JWT claims to database queries
2. `DatabaseAuthorizationService.cs` - Implement user lookup and permission checking  
3. `UserResolutionMiddleware.cs` - Auto-create users on first API call

**Success Criteria**: Davis can log in via Auth0 and access all admin functionality through database permissions.

### Auth0 Configuration Ready ✅

- **RBAC**: Can be disabled (all permissions now in database)
- **Permissions**: Can be deleted (using database permissions)  
- **Roles**: Can be deleted (using database roles)
- **JWT Tokens**: Only need `sub` claim for user identification

The foundation is complete and tested. Tomorrow focuses on the authorization service implementation to complete the transition.
