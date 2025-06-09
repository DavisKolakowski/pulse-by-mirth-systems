# Auth0 Setup Guide for Pulse API

## Overview
This guide will walk you through setting up Auth0 for the Pulse application, including creating the API, defining permissions, creating roles, and assigning permissions to roles.

## Important Note: Public vs. Authenticated Access
The Pulse platform is designed to be publicly accessible for venue and special discovery - users should be able to browse venues, see current specials, and view live activity threads without needing to register or authenticate. 

**Public Access (No Authentication Required):**
- Browse venues and their information
- View current specials and events
- Read live activity threads and user posts
- Search and filter venues by location, type, tags, and vibes

**Authenticated Access (Permissions Required):**
- Venue management (creating, updating, deleting venues)
- Special management (creating, updating, deleting specials)
- Content moderation and administrative functions
- Analytics access
- User and role management

## Step 1: Create the Pulse API in Auth0

1. **Navigate to APIs**:
   - Go to your Auth0 Dashboard
   - Click on "APIs" in the left sidebar
   - Click "Create API"

2. **Configure the API**:
   - **Name**: `Pulse API`
   - **Identifier**: `https://pulse-api.com` (or your actual API URL)
   - **Signing Algorithm**: `RS256`
   - Click "Create"

## Step 2: Create Pulse API Permissions

Navigate to your newly created Pulse API and go to the "Permissions" tab. Add the following permissions:

### Venue Management Permissions
- **Permission**: `read:venues`
  - **Description**: Query and view venue information (scope determined by role)
- **Permission**: `write:venues`
  - **Description**: Create and update venue information (scope determined by role)
- **Permission**: `delete:venues`
  - **Description**: Delete venues from the system (scope determined by role)

### Special Management Permissions
- **Permission**: `write:specials`
  - **Description**: Create and update special offers and events
- **Permission**: `delete:specials`
  - **Description**: Delete special offers and events

### Content Management Permissions
- **Permission**: `read:content`
  - **Description**: Read all user-generated content and posts
- **Permission**: `write:content`
  - **Description**: Create and update content across all venues
- **Permission**: `delete:content`
  - **Description**: Delete inappropriate or violating content
- **Permission**: `moderate:content`
  - **Description**: Moderate user posts and venue content

### Analytics Permissions
- **Permission**: `read:analytics`
  - **Description**: Access venue analytics and performance metrics
- **Permission**: `read:analytics-global`
  - **Description**: Access global platform analytics and insights

### Tag Management Permissions
- **Permission**: `read:tags`
  - **Description**: Read venue tags and tag definitions
- **Permission**: `write:tags`
  - **Description**: Create and update venue tags
- **Permission**: `delete:tags`
  - **Description**: Delete tags from the system
- **Permission**: `manage:tag-assignments`
  - **Description**: Assign and remove tags from venues

### Vibe Management Permissions
- **Permission**: `read:vibes`
  - **Description**: Read venue vibes and atmosphere descriptors
- **Permission**: `write:vibes`
  - **Description**: Create and update venue vibes
- **Permission**: `delete:vibes`
  - **Description**: Delete vibes from the system

### Post and Thread Management Permissions
- **Permission**: `read:posts`
  - **Description**: Read live activity posts and threads
- **Permission**: `write:posts`
  - **Description**: Create posts in venue activity threads
- **Permission**: `delete:posts`
  - **Description**: Delete posts from activity threads
- **Permission**: `moderate:posts`
  - **Description**: Moderate posts and manage thread activity

### Venue Category Management Permissions
- **Permission**: `read:venue-categories`
  - **Description**: Read venue category definitions and classifications
- **Permission**: `write:venue-categories`
  - **Description**: Create and update venue category definitions
- **Permission**: `delete:venue-categories`
  - **Description**: Delete venue categories from the system

### Notification Management Permissions
- **Permission**: `read:notifications`
  - **Description**: Access user notification settings and history
- **Permission**: `write:notifications`
  - **Description**: Send notifications to users
- **Permission**: `manage:notification-preferences`
  - **Description**: Manage user notification preferences and tag follows

### Media Management Permissions
- **Permission**: `read:media`
  - **Description**: Access venue photos and media content
- **Permission**: `write:media`
  - **Description**: Upload and manage venue photos and media
- **Permission**: `delete:media`
  - **Description**: Delete media content from venues

### Tag and Vibe Management Permissions
- **Permission**: `read:tags`
  - **Description**: Read tag definitions and assignments
- **Permission**: `write:tags`
  - **Description**: Create and update tags for specials
- **Permission**: `delete:tags`
  - **Description**: Delete or consolidate tags
- **Permission**: `moderate:tags`
  - **Description**: Feature, hide, or manage tag usage across platform
- **Permission**: `read:vibes`
  - **Description**: Read vibe definitions and current venue vibes
- **Permission**: `write:vibes`
  - **Description**: Create vibes in user posts
- **Permission**: `moderate:vibes`
  - **Description**: Moderate vibe content for appropriateness

### Post and Activity Thread Permissions
- **Permission**: `write:posts`
  - **Description**: Create posts in venue activity threads
- **Permission**: `delete:posts`
  - **Description**: Delete posts (own posts or moderation)
- **Permission**: `moderate:posts`
  - **Description**: Moderate user posts across all venues

### Media Management Permissions
- **Permission**: `upload:media`
  - **Description**: Upload photos and short videos to venue profiles and posts
- **Permission**: `delete:media`
  - **Description**: Delete media content
- **Permission**: `moderate:media`
  - **Description**: Moderate media content for appropriateness

### Notification and Following Permissions
- **Permission**: `read:notifications`
  - **Description**: Read user notifications
- **Permission**: `write:notifications`
  - **Description**: Send notifications to users
- **Permission**: `manage:follows`
  - **Description**: Follow/unfollow tags and venues for notifications

### Venue Type Management Permissions
- **Permission**: `read:venue-types`
  - **Description**: Read available venue type categories
- **Permission**: `write:venue-types`
  - **Description**: Create and update venue type definitions
- **Permission**: `delete:venue-types`
  - **Description**: Remove venue types from the system

### User Management Permissions (Venue-specific)
- **Permission**: `read:venue-users`
  - **Description**: Read venue user assignments and permissions
- **Permission**: `write:venue-users`
  - **Description**: Assign users to venues and manage venue-specific permissions
- **Permission**: `delete:venue-users`
  - **Description**: Remove users from venue assignments

### System Administration Permissions
- **Permission**: `admin:system`
  - **Description**: Full system administration access
- **Permission**: `config:system`
  - **Description**: Modify system configuration and settings

## Step 3: Enable Auth0 Management API

1. **Navigate to APIs**:
   - In your Auth0 Dashboard, go to "APIs"
   - Find "Auth0 Management API" (this should already exist)
   - Click on it

2. **Note the Management API Permissions**:
   The following permissions from the Management API will be used:
   - `read:users` - Read user information
   - `update:users` - Update user information  
   - `create:users` - Create new users
   - `delete:users` - Delete users (Administrator only)

## Step 4: Create Roles

Navigate to "User Management" > "Roles" and create the following roles:

### 1. Administrator Role
- **Name**: `administrator`
- **Description**: Full global application administration access with complete system control

### 2. Content Manager Role  
- **Name**: `content-manager`
- **Description**: Manage all venues, content, and platform-wide moderation

### 3. Venue Owner Role
- **Name**: `venue-owner` 
- **Description**: Full management access for assigned venues including user management

### 4. Venue Manager Role
- **Name**: `venue-manager`
- **Description**: Manage specials and content for assigned venues

## Step 5: Assign Permissions to Roles

### Administrator Role Permissions

**From Pulse API:**

- `read:venues`
- `write:venues`
- `delete:venues`
- `write:specials`
- `delete:specials`
- `read:content`
- `write:content`
- `delete:content`
- `moderate:content`
- `write:posts`
- `delete:posts`
- `moderate:posts`
- `upload:media`
- `delete:media`
- `moderate:media`
- `read:tags`
- `write:tags`
- `delete:tags`
- `moderate:tags`
- `read:vibes`
- `write:vibes`
- `moderate:vibes`
- `read:venue-types`
- `write:venue-types`
- `delete:venue-types`
- `read:analytics`
- `read:analytics-global`
- `read:notifications`
- `write:notifications`
- `manage:follows`
- `read:venue-users`
- `write:venue-users`
- `delete:venue-users`
- `admin:system`
- `config:system`

**From Auth0 Management API:**

- `read:users`
- `update:users`
- `create:users`
- `delete:users`

### Content Manager Role Permissions

**From Pulse API:**

- `read:venues`
- `write:venues`
- `write:specials`
- `read:content`
- `write:content`
- `delete:content`
- `moderate:content`
- `write:posts`
- `delete:posts`
- `moderate:posts`
- `upload:media`
- `delete:media`
- `moderate:media`
- `read:tags`
- `write:tags`
- `moderate:tags`
- `read:vibes`
- `moderate:vibes`
- `read:venue-types`
- `read:analytics`

**From Auth0 Management API:**

- `read:users`
- `update:users`

### Venue Owner Role Permissions

**From Pulse API:**

- `read:venues`
- `write:venues`
- `delete:venues`
- `write:specials`
- `delete:specials`
- `read:content`
- `write:content`
- `write:posts`
- `upload:media`
- `read:tags`
- `write:tags`
- `read:vibes`
- `write:vibes`
- `read:venue-types`
- `read:analytics`
- `read:notifications`
- `write:notifications`
- `manage:follows`
- `read:venue-users`
- `write:venue-users`
- `delete:venue-users`

**From Auth0 Management API:**

- `read:users`
- `update:users`

### Venue Manager Role Permissions

**From Pulse API:**

- `read:venues`
- `write:specials`
- `delete:specials`
- `read:content`
- `write:content`
- `write:posts`
- `upload:media`
- `read:tags`
- `write:tags`
- `read:vibes`
- `write:vibes`
- `read:venue-types`
- `read:analytics`
- `read:notifications`

**From Auth0 Management API:**

- `read:users`

## Step 7: Understand the Authorization System

The Pulse application uses a simple and elegant authorization system:

1. **Automatic Policy Creation**: Each Auth0 permission automatically becomes a policy with the same name
2. **No Configuration Required**: The system dynamically creates policies based on permissions in the user's token
3. **Role-Based Venue Scoping**: Venue-specific access is handled through role-based authorization handlers

### Your appsettings.json Configuration

Your `appsettings.json` should remain simple and only contain the essential Auth0 configuration:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "PORT": "6060",
  "CLIENT_ORIGIN_URL": "http://localhost:4040",
  "AUTH0_AUDIENCE": "https://pulse.mirthsystems.com",
  "AUTH0_DOMAIN": "mirthsystems.us.auth0.com"
}
```

### How Permission Policies Work

The system automatically creates policies for each Auth0 permission with the same name:

- `read:venues` permission → `read:venues` policy
- `write:venues` permission → `write:venues` policy  
- `delete:venues` permission → `delete:venues` policy
- `write:specials` permission → `write:specials` policy
- `delete:specials` permission → `delete:specials` policy
- And so on for all permissions...

### Benefits of This Authorization System

1. **No Configuration Required**: Permission policies are created automatically - no appsettings.json configuration needed
2. **Direct Permission Mapping**: Each Auth0 permission automatically becomes a policy with the same name
3. **Simple Controller Usage**: Use permissions directly as policy names: `[Authorize(Policy = "read:venues")]`
4. **Dynamic Policy Creation**: Adding new permissions in Auth0 automatically creates corresponding policies
5. **Clean and Maintainable**: No complex configuration files to manage

## Step 8: Implement Hybrid Identity Architecture

### Database Schema for Venue Assignments

Create the following tables to manage venue-specific role assignments:

```sql
-- User table linking to Auth0
CREATE TABLE Users (
    Id BIGSERIAL PRIMARY KEY,
    Auth0UserId VARCHAR(255) UNIQUE NOT NULL,
    Email VARCHAR(255) NOT NULL,
    CreatedAt TIMESTAMP NOT NULL DEFAULT NOW(),
    UpdatedAt TIMESTAMP NOT NULL DEFAULT NOW(),
    LastLoginAt TIMESTAMP,
    IsActive BOOLEAN NOT NULL DEFAULT true
);

-- Venue role assignments
CREATE TABLE VenueRoles (
    Id BIGSERIAL PRIMARY KEY,
    UserId BIGINT NOT NULL REFERENCES Users(Id),
    VenueId BIGINT NOT NULL REFERENCES Venues(Id),
    RoleName VARCHAR(50) NOT NULL,
    AssignedAt TIMESTAMP NOT NULL DEFAULT NOW(),
    AssignedBy BIGINT REFERENCES Users(Id),
    IsActive BOOLEAN NOT NULL DEFAULT true,
    UNIQUE(UserId, VenueId, RoleName)
);

-- Index for performance
CREATE INDEX idx_venue_roles_user_venue ON VenueRoles(UserId, VenueId) WHERE IsActive = true;
CREATE INDEX idx_venue_roles_venue ON VenueRoles(VenueId) WHERE IsActive = true;
```

### User Synchronization Flow

1. **User Registration/Login**: 
   - User authenticates with Auth0
   - API checks if user exists in local database
   - If not exists, create user record with Auth0UserId
   - Update LastLoginAt timestamp

2. **Role Assignment**:
   - Administrators use API endpoints to assign Auth0 roles to users for specific venues
   - VenueRoles table stores which users have which roles for which venues
   - Role verification queries Auth0 Management API to ensure role exists

3. **Authorization Flow**:
   - JWT token contains permissions from Auth0
   - For venue-scoped operations, query VenueRoles table to filter allowed venues
   - Apply row-level security based on user's venue assignments

## Step 9: Update Application Settings

1. **Navigate to Applications**:   - Go to "APIs" > "Pulse API"
   - Click on the "Settings" tab

2. **Enable RBAC**:
   - Toggle "Enable RBAC" to ON
   - Toggle "Add Permissions in the Access Token" to ON

3. **Configure Token Settings**:
   - Set Token Expiration as needed (recommend 24 hours for access tokens)
   - Configure refresh token settings if using refresh tokens

## Step 10: Configure Application Settings

1. **Navigate to Applications**:
   - Go to "Applications" in the Auth0 Dashboard
   - Select your Pulse application

2. **Configure Application**:
   - Ensure the application type is set correctly (SPA for React app, Regular Web App for backend)
   - Add your callback URLs, logout URLs, and allowed origins
   - In the "APIs" tab, authorize your application to access both:
     - The Pulse API
     - The Auth0 Management API (with appropriate scopes)

## Step 12: Understanding the Simple Authorization Flow

### How It Works

1. **Token Validation**: Verify JWT signature and extract permissions from token
2. **Permission Check**: For permission-based policies, verify user has the permission in their token  
3. **Venue Scoping**: For venue-specific operations, additional handlers apply row-level security
4. **Authorization Decision**: Allow/deny access with appropriate data scope

### Examples of Adding New Features

When you want to add a new feature:

1. **Add Permission to Auth0**: Create `export:analytics` permission in Auth0 dashboard
2. **Use Immediately**: Apply to controller: `[Authorize(Policy = "export:analytics")]`
3. **Assign to Roles**: Update role assignments in Auth0 to include the new permission
4. **Deploy**: No configuration changes needed - the policy is automatically available

```csharp
// New permission added to Auth0: export:analytics
[Authorize(Policy = "export:analytics")]
public async Task<IActionResult> ExportAnalytics(int venueId)
{
    // Immediately usable - no appsettings.json configuration required
    // Permission-to-policy mapping is automatic
}
```

## Important Notes

- **Hybrid Authorization Model**: Auth0 handles application-level roles and permissions, while your database handles venue-specific assignments
- **Token Validation**: Your API should validate both the token signature and the permissions within the token
- **Permission Enforcement**: Implement middleware in your .NET API to check both permission-based and role-based policies
- **Venue Assignment Management**: Venue assignments should be managed through your application's admin interface
- **Configuration Updates**: Policy changes in appsettings.json take effect immediately without redeployment

## Security Considerations

1. **Principle of Least Privilege**: Only assign the minimum permissions necessary for each role
2. **Regular Audits**: Periodically review user roles and venue assignments
3. **Token Security**: Implement proper token storage and handling in your frontend
4. **API Security**: Use HTTPS and implement rate limiting
5. **Database Security**: Ensure venue assignments are properly validated and audited
6. **Configuration Security**: Protect appsettings.json and use environment-specific configurations

## Practical Implementation Examples

### Controller Authorization Examples

```csharp
// Direct permission-based authorization (automatically configured)
[Authorize(Policy = "read:venues")]
[HttpGet]
public async Task<IActionResult> GetAllVenues()
{
    // Available to any user with read:venues permission
    // No venue scoping applied
}

// Role-based authorization with venue scoping (configured in appsettings.json)
[Authorize(Policy = "VenueOwnerPolicy")]
[HttpPut("venues/{id}")]
public async Task<IActionResult> UpdateVenue(int id, VenueUpdateRequest request)
{
    // Only venue owners can access
    // Automatically filtered to their assigned venues
}

// Permission-based for global operations
[Authorize(Policy = "write:specials")]
[HttpPost("venues/{venueId}/specials")]
public async Task<IActionResult> CreateSpecial(int venueId, SpecialCreateRequest request)
{
    // Any user with write:specials permission
    // Venue scoping determined by user's role
}

// Multiple options - permission or role-based
[Authorize(Policy = "moderate:content")]  // Global content moderators
// OR
[Authorize(Policy = "VenueOwnerPolicy")]   // Venue owners (scoped to their venues)
[HttpDelete("content/{id}")]
public async Task<IActionResult> DeleteContent(int id)
{
    // Accessible via multiple authorization paths
}
```

### Adding New Permissions

To add a new permission:

1. **Add to Auth0**: Create the new permission in Auth0 dashboard (e.g., `approve:specials`)
2. **Use Immediately**: The policy is automatically available: `[Authorize(Policy = "approve:specials")]`
3. **Assign to Roles**: Update role assignments in Auth0 to include the new permission
4. **No Code Changes**: The system automatically recognizes and handles the new permission

### Common Usage Patterns

```csharp
// Individual permission policies (automatically available)
[Authorize(Policy = "read:venues")]        // Read venue data
[Authorize(Policy = "write:venues")]       // Create/update venues  
[Authorize(Policy = "delete:venues")]      // Delete venues
[Authorize(Policy = "write:specials")]     // Create/update specials
[Authorize(Policy = "delete:specials")]    // Delete specials
[Authorize(Policy = "moderate:content")]   // Moderate user content
[Authorize(Policy = "read:analytics")]     // View analytics
[Authorize(Policy = "admin:system")]       // System administration

// Role-based policies (configured in appsettings.json)
[Authorize(Policy = "VenueOwnerPolicy")]   // Venue owners only, scoped to their venues
[Authorize(Policy = "VenueManagerPolicy")] // Venue managers only, scoped to their venues
[Authorize(Policy = "AdminPolicy")]        // Administrators only, global access
```

This architecture provides a clean separation between:
- **Permission-based policies**: Direct mapping from Auth0 permissions for API access control
- **Role-based policies**: Business logic for venue-specific access and row-level security

This completes the Auth0 setup for the Pulse application with a flexible, configurable authorization system that supports both global permissions and venue-specific access control.
