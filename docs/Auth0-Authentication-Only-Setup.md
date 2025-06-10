# Simplified Auth0 Setup Guide for Pulse (Authentication Only)

## Overview

This guide covers setting up Auth0 for **authentication only** in the Pulse application. All authorization (roles, permissions, access control) is handled by our PostgreSQL database.

**Auth0 Responsibilities:**
- User login/registration
- JWT token generation
- User identity verification

**Database Responsibilities:**
- All roles and permissions
- Venue-specific access control
- Authorization decisions

## Step 1: Create Auth0 Application

1. **Navigate to Applications**:
   - Go to your Auth0 Dashboard
   - Click "Applications" in the left sidebar
   - Click "Create Application"

2. **Configure Application**:
   - **Name**: `Pulse Web App`
   - **Type**: `Single Page Web Application` (for React frontend)
   - Click "Create"

3. **Configure Application Settings**:
   - **Allowed Callback URLs**: `http://localhost:4040/callback`
   - **Allowed Logout URLs**: `http://localhost:4040`
   - **Allowed Web Origins**: `http://localhost:4040`
   - **Allowed Origins (CORS)**: `http://localhost:4040`

## Step 2: Create Auth0 API

1. **Navigate to APIs**:
   - Go to "APIs" in the left sidebar
   - Click "Create API"

2. **Configure API**:
   - **Name**: `Pulse API`
   - **Identifier**: `https://pulse-api.com` (or your actual API URL)
   - **Signing Algorithm**: `RS256`
   - Click "Create"

3. **API Settings**:
   - **Enable RBAC**: `OFF` (we don't use Auth0 roles)
   - **Add Permissions in Access Token**: `OFF` (we don't use Auth0 permissions)

## Step 3: Configure JWT Token

Your JWT tokens will be minimal and contain only essential user information:

```json
{
  "sub": "auth0|682e1f2e2121380bbeb56dcf",
  "email": "davis_kolakowski@mirthsystems.com", 
  "aud": "https://pulse-api.com",
  "iss": "https://mirthsystems.us.auth0.com/",
  "iat": 1640995200,
  "exp": 1641081600
}
```

**Key Fields:**
- `sub`: User's Auth0 ID (used to look up user in database)
- `email`: User's email address
- No permissions or roles (handled by database)

## Step 4: Database User Management

### User Creation Flow

1. **User logs in via Auth0** → Gets JWT token with `sub` claim
2. **API receives request** → Validates JWT and extracts `sub` claim  
3. **API queries database** → Looks up user by `provider_id = sub`
4. **If user doesn't exist** → Auto-create user record:

```csharp
public async Task<User> GetOrCreateUserAsync(string providerId, string email)
{
    var user = await _context.Users
        .FirstOrDefaultAsync(u => u.ProviderId == providerId);
    
    if (user == null)
    {
        user = new User
        {
            ProviderId = providerId,
            Email = email,
            CreatedAt = SystemClock.Instance.GetCurrentInstant(),
            IsActive = true
        };
        
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }
    
    user.LastLoginAt = SystemClock.Instance.GetCurrentInstant();
    await _context.SaveChangesAsync();
    
    return user;
}
```

### Seeded Admin User

The system includes Davis Kolakowski as the initial administrator:

```sql
-- Initial admin user
INSERT INTO users (id, provider_id, email, first_name, last_name) VALUES
(1, 'auth0|682e1f2e2121380bbeb56dcf', 'davis_kolakowski@mirthsystems.com', 'Davis', 'Kolakowski');

-- Assign Administrator role
INSERT INTO user_roles (user_id, role_id, assigned_by_user_id) VALUES
(1, 1, 1); -- Davis gets Administrator role (self-assigned during initialization)
```

## Step 5: Application Configuration

### Backend (ASP.NET Core)

**appsettings.json:**
```json
{
  "Auth0": {
    "Domain": "mirthsystems.us.auth0.com",
    "Audience": "https://pulse-api.com"
  }
}
```

**Program.cs:**
```csharp
// JWT Authentication (Auth0)
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = $"https://{builder.Configuration["Auth0:Domain"]}/";
        options.Audience = builder.Configuration["Auth0:Audience"];
        options.TokenValidationParameters = new TokenValidationParameters
        {
            NameClaimType = ClaimTypes.NameIdentifier
        };
    });

// Database Authorization (Custom)
builder.Services.AddScoped<IAuthorizationService, DatabaseAuthorizationService>();

// Auto user creation middleware
builder.Services.AddScoped<UserResolutionMiddleware>();
```

### Frontend (React)

**auth0-provider.tsx:**
```typescript
import { Auth0Provider } from '@auth0/auth0-react';

export const Auth0ProviderWithNavigate = ({ children }: Props) => {
  return (
    <Auth0Provider
      domain="mirthsystems.us.auth0.com"
      clientId="YOUR_CLIENT_ID"
      authorizationParams={{
        redirect_uri: window.location.origin + "/callback",
        audience: "https://pulse-api.com"
      }}
    >
      {children}
    </Auth0Provider>
  );
};
```

## Step 6: Testing Authentication

### Test User Login
1. Start your application
2. Navigate to login page
3. User should be redirected to Auth0
4. After successful login, user should be redirected back with JWT token
5. API should auto-create user record in database

### Test API Authorization
```csharp
[Authorize] // Just checks for valid JWT token
[HttpGet("profile")]
public async Task<IActionResult> GetProfile()
{
    var user = await _authService.GetCurrentUserAsync();
    return Ok(user);
}

[Authorize]
[HttpGet("venues")]
public async Task<IActionResult> GetVenues()
{
    // Database checks if user has read:venues permission
    var hasPermission = await _authService.HasPermissionAsync("read:venues");
    if (!hasPermission)
        return Forbid();
    
    var venues = await _venueService.GetVenuesAsync();
    return Ok(venues);
}
```

## Step 7: User Management

### Role Assignment (Database Only)

```csharp
// Assign global role (Administrator, Content Manager)
public async Task AssignGlobalRoleAsync(long userId, int roleId)
{
    var userRole = new UserRole
    {
        UserId = userId,
        RoleId = roleId,
        AssignedAt = SystemClock.Instance.GetCurrentInstant(),
        IsActive = true
    };
    
    _context.UserRoles.Add(userRole);
    await _context.SaveChangesAsync();
}

// Assign venue-specific role (Venue Owner, Venue Manager)
public async Task AssignVenueRoleAsync(long userId, long venueId, int roleId)
{
    var venueRole = new VenueRole
    {
        UserId = userId,
        VenueId = venueId,
        RoleId = roleId,
        AssignedAt = SystemClock.Instance.GetCurrentInstant(),
        IsActive = true
    };
    
    _context.VenueRoles.Add(venueRole);
    await _context.SaveChangesAsync();
}
```

## What We DON'T Need from Auth0

❌ **Auth0 Permissions** - All permissions are in database  
❌ **Auth0 Roles** - All roles are in database  
❌ **Auth0 Management API** - No role/permission management needed  
❌ **RBAC Settings** - Keep RBAC disabled in Auth0  
❌ **Custom Claims** - JWT contains only standard claims  
❌ **Actions/Rules** - No custom logic needed in Auth0  

## Benefits of This Approach

✅ **Simplified Auth0 Setup** - Minimal configuration required  
✅ **Lower Auth0 Costs** - No Management API usage or per-user permissions  
✅ **Complete Control** - All authorization logic in your database  
✅ **Better Performance** - No external API calls for authorization  
✅ **Easier Testing** - Database authorization is easier to test  
✅ **Venue Context** - Native support for venue-specific permissions  

## Troubleshooting

### Common Issues

**JWT Token Missing `sub` Claim**
- Check Auth0 API configuration
- Ensure application is authorized to access the API

**User Not Created in Database**
- Verify `UserResolutionMiddleware` is registered
- Check database connection string
- Confirm `GetOrCreateUserAsync` is being called

**Authorization Always Fails**
- Verify user exists in database
- Check role assignments in `user_roles` table
- Confirm permission checks are using correct permission names

### Debug JWT Token
```csharp
// Add to controller to debug token contents
[HttpGet("debug-token")]
public IActionResult DebugToken()
{
    var claims = User.Claims.Select(c => new { c.Type, c.Value });
    return Ok(claims);
}
```

This completes the simplified Auth0 setup for authentication-only usage with the Pulse application.
