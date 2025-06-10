# Pulse Authentication and Authorization Documentation

## Overview
This document provides a comprehensive reference for the hybrid authentication and authorization system used in the Pulse application. The system combines Auth0 for authentication and permission management with a provider-agnostic database design for venue-specific access control.

## Hybrid Authorization Architecture

Pulse implements a **hybrid approach** that combines the best of both worlds:

### 🔐 **Auth0 Integration**
- **Authentication**: Secure user authentication with JWT tokens
- **Global Permissions**: Centralized permission management via Auth0 dashboard
- **Auto-Policy Creation**: Auth0 permissions automatically become authorization policies
- **Token-Based**: All permissions included in JWT access tokens

### 🗄️ **Database-Driven Role Management**  
- **Provider-Agnostic Design**: Users identified by `ProviderId` (works with Auth0, Azure AD, etc.)
- **Venue-Specific Roles**: Fine-grained access control via `VenueRole` entity
- **Relationship Mapping**: Direct user-to-venue assignments in the database
- **Flexible Architecture**: Easy to extend for multiple auth providers

### ⚡ **Simple Controller Usage**
```csharp
// Auto-created policy from Auth0 permission
[Authorize(Policy = "read:venues")]
public async Task<IActionResult> GetVenues() { }

// Venue-specific access controlled by database roles
[Authorize(Policy = "write:specials")]
public async Task<IActionResult> CreateSpecial(int venueId) 
{
    // Additional venue-level authorization via RBACHandler
}
```

The system uses a combination of Auth0 permissions for global access control and database entities for venue-specific role management to provide comprehensive, scalable authorization.

## Important Note: Public vs. Authenticated Access
The Pulse platform prioritizes public accessibility for venue discovery. Most core functionality is available without authentication to encourage user adoption and provide immediate value.

**Public Access (No Authentication Required):**
- Browse venues and their information
- View current specials and events  
- Read live activity threads and user posts
- Search and filter venues by location, type, tags, and vibes
- View analytics data (anonymized, public metrics)

**Authenticated Access (Permissions Required):**
- Venue management (creating, updating, deleting venues)
- Special management (creating, updating, deleting specials)
- Content moderation and administrative functions
- User and role management
- Private analytics and venue-specific data

## API Resources

### Pulse API
- **Identifier**: `https://pulse-api.com`
- **Description**: Custom API for the Pulse nightlife discovery platform
- **Signing Algorithm**: RS256

### Auth0 Management API  
- **Identifier**: `https://{tenant}.auth0.com/api/v2/`
- **Description**: Auth0's Management API for user management operations
- **Signing Algorithm**: RS256

## Permissions Reference

### Pulse API Permissions

#### Venue Management
| Permission | Name | Description |
|------------|------|-------------|
| `read:venues` | Read Venues | Query and view venue information (scope determined by role) |
| `write:venues` | Write Venues | Create and update venue information (scope determined by role) |
| `delete:venues` | Delete Venues | Delete venues from the system (scope determined by role) |

#### Special Management  
| Permission | Name | Description |
|------------|------|-------------|
| `write:specials` | Write Specials | Create and update special offers (scope determined by venue access) |
| `delete:specials` | Delete Specials | Delete special offers (scope determined by venue access) |

#### Content Management
| Permission | Name | Description |
|------------|------|-------------|
| `read:content` | Read Content | Read all user-generated content and posts |
| `write:content` | Write Content | Create and update content across all venues |
| `delete:content` | Delete Content | Delete inappropriate or violating content |
| `moderate:content` | Moderate Content | Moderate user posts and venue content |

#### Analytics Access
| Permission | Name | Description |
|------------|------|-------------|
| `read:analytics` | Read Analytics | Access venue analytics and performance metrics |
| `read:analytics-global` | Global Analytics | Access global platform analytics and insights |

#### Tag and Vibe Management
| Permission | Name | Description |
|------------|------|-------------|
| `read:tags` | Read Tags | Read tag definitions and assignments |
| `write:tags` | Write Tags | Create and update tags for specials |
| `delete:tags` | Delete Tags | Delete or consolidate tags |
| `moderate:tags` | Moderate Tags | Feature, hide, or manage tag usage across platform |
| `read:vibes` | Read Vibes | Read vibe definitions and current venue vibes |
| `write:vibes` | Write Vibes | Create vibes in user posts |
| `moderate:vibes` | Moderate Vibes | Moderate vibe content for appropriateness |

#### Post and Activity Thread Management
| Permission | Name | Description |
|------------|------|-------------|
| `write:posts` | Write Posts | Create posts in venue activity threads |
| `delete:posts` | Delete Posts | Delete posts (own posts or moderation) |
| `moderate:posts` | Moderate Posts | Moderate user posts across all venues |

#### Media Management
| Permission | Name | Description |
|------------|------|-------------|
| `upload:media` | Upload Media | Upload photos and short videos to venue profiles and posts |
| `delete:media` | Delete Media | Delete media content |
| `moderate:media` | Moderate Media | Moderate media content for appropriateness |

#### Notification and Following Management
| Permission | Name | Description |
|------------|------|-------------|
| `read:notifications` | Read Notifications | Read user notifications |
| `write:notifications` | Write Notifications | Send notifications to users |
| `manage:follows` | Manage Follows | Follow/unfollow tags and venues for notifications |

#### Venue Category Management
| Permission | Name | Description |
|------------|------|-------------|
| `read:venue-categories` | Read Venue Categories | Read available venue category classifications |
| `write:venue-categories` | Write Venue Categories | Create and update venue category definitions |
| `delete:venue-categories` | Delete Venue Categories | Remove venue categories from the system |

#### Venue User Management
| Permission | Name | Description |
|------------|------|-------------|
| `read:venue-users` | Read Venue Users | Read venue user assignments and permissions |
| `write:venue-users` | Write Venue Users | Assign users to venues and manage venue-specific permissions |
| `delete:venue-users` | Delete Venue Users | Remove users from venue assignments |

#### System Administration
| Permission | Name | Description |
|------------|------|-------------|
| `admin:system` | System Admin | Full system administration access |
| `config:system` | System Config | Modify system configuration and settings |

### Auth0 Management API Permissions

| Permission | Name | Description |
|------------|------|-------------|
| `read:users` | Read Users | Read user profile information |
| `update:users` | Update Users | Update user profile information |
| `create:users` | Create Users | Create new user accounts |
| `delete:users` | Delete Users | Delete user accounts (Administrator only) |

## Roles Reference

### Administrator
- **Role Name**: `administrator`
- **Description**: Full global application administration access with complete system control
- **Use Case**: System administrators who need complete access to all platform features

#### Assigned Permissions

**Pulse API Permissions:**

- `read:venues` - Read all venues globally
- `write:venues` - Create and update any venue
- `delete:venues` - Delete any venue from the system
- `write:specials` - Create and update any special offer
- `delete:specials` - Delete any special offer
- `read:content` - Read all user content
- `write:content` - Create content on any venue
- `delete:content` - Delete any content
- `moderate:content` - Moderate all platform content
- `write:posts` - Create posts in activity threads
- `delete:posts` - Delete any posts
- `moderate:posts` - Moderate user posts across all venues
- `upload:media` - Upload photos and videos
- `delete:media` - Delete any media content
- `moderate:media` - Moderate media content for appropriateness
- `read:tags` - Read all tag definitions and assignments
- `write:tags` - Create and update any tags
- `delete:tags` - Delete or consolidate tags
- `moderate:tags` - Manage tag usage across platform
- `read:vibes` - Read all vibe definitions
- `write:vibes` - Create vibes in posts
- `moderate:vibes` - Moderate vibe content
- `read:venue-categories` - Read venue category classifications
- `write:venue-categories` - Create and update venue categories
- `delete:venue-categories` - Remove venue categories
- `read:analytics` - Access all venue analytics
- `read:analytics-global` - Access global platform analytics
- `read:notifications` - Read all notifications
- `write:notifications` - Send notifications to users
- `manage:follows` - Manage user follows and subscriptions
- `read:venue-users` - View all venue user assignments
- `write:venue-users` - Manage all venue user assignments
- `delete:venue-users` - Remove users from any venue
- `admin:system` - Full system administration
- `config:system` - Modify system configuration

**Auth0 Management API Permissions:**
- `read:users` - Read user information
- `update:users` - Update user profiles
- `create:users` - Create new users
- `delete:users` - Delete user accounts

### Content Manager
- **Role Name**: `content-manager`
- **Description**: Manage all venues, content, and platform-wide moderation
- **Use Case**: Staff members responsible for content moderation and venue management

#### Assigned Permissions

**Pulse API Permissions:**

- `read:venues` - Read all venues globally
- `write:venues` - Create and update venues
- `write:specials` - Create and update special offers
- `read:content` - Read all user content
- `write:content` - Create content on venues
- `delete:content` - Delete inappropriate content
- `moderate:content` - Moderate platform content
- `write:posts` - Create posts in activity threads
- `delete:posts` - Delete inappropriate posts
- `moderate:posts` - Moderate user posts
- `upload:media` - Upload photos and videos
- `delete:media` - Delete inappropriate media
- `moderate:media` - Moderate media content
- `read:tags` - Read tag definitions and assignments
- `write:tags` - Create and update tags
- `moderate:tags` - Manage tag usage
- `read:vibes` - Read vibe definitions
- `moderate:vibes` - Moderate vibe content
- `read:venue-categories` - Read venue category classifications
- `read:analytics` - Access venue analytics

**Auth0 Management API Permissions:**
- `read:users` - Read user information
- `update:users` - Update user profiles

### Venue Owner
- **Role Name**: `venue-owner`
- **Description**: Full management access for assigned venues including user management
- **Use Case**: Business owners who own one or more venues and need to manage them and their staff

#### Assigned Permissions

**Pulse API Permissions:**

- `read:venues` - Read venue information (limited to assigned venues via role-based policy)
- `write:venues` - Update venue information (limited to assigned venues via role-based policy)
- `delete:venues` - Delete venues (limited to assigned venues via role-based policy)
- `write:specials` - Create and update specials (limited to assigned venues via role-based policy)
- `delete:specials` - Delete specials (limited to assigned venues via role-based policy)
- `read:content` - Read content (limited to assigned venues via role-based policy)
- `write:content` - Create content (limited to assigned venues via role-based policy)
- `write:posts` - Create posts in venue activity threads (limited to assigned venues)
- `upload:media` - Upload photos and videos (limited to assigned venues)
- `read:tags` - Read tag definitions and assignments
- `write:tags` - Create and assign tags to specials (limited to assigned venues)
- `read:vibes` - Read vibe definitions
- `write:vibes` - Create vibes in posts (limited to assigned venues)
- `read:venue-types` - Read venue type categories
- `read:analytics` - Access analytics (limited to assigned venues via role-based policy)
- `read:notifications` - Read notifications
- `write:notifications` - Send notifications related to assigned venues
- `manage:follows` - Manage venue-related follows and subscriptions
- `read:venue-users` - View venue user assignments (limited to assigned venues via role-based policy)
- `write:venue-users` - Manage venue user assignments (limited to assigned venues via role-based policy)
- `delete:venue-users` - Remove users from venues (limited to assigned venues via role-based policy)

**Auth0 Management API Permissions:**
- `read:users` - Read user information
- `update:users` - Update user profiles

### Venue Manager
- **Role Name**: `venue-manager`
- **Description**: Manage specials and content for assigned venues
- **Use Case**: Staff members who manage day-to-day operations for specific venues

#### Assigned Permissions

**Pulse API Permissions:**

- `read:venues` - Read venue information (limited to assigned venues via role-based policy)
- `write:specials` - Create and update specials (limited to assigned venues via role-based policy)
- `delete:specials` - Delete specials (limited to assigned venues via role-based policy)
- `read:content` - Read content (limited to assigned venues via role-based policy)
- `write:content` - Create content (limited to assigned venues via role-based policy)
- `write:posts` - Create posts in venue activity threads (limited to assigned venues)
- `upload:media` - Upload photos and videos (limited to assigned venues)
- `read:tags` - Read tag definitions and assignments
- `write:tags` - Create and assign tags to specials (limited to assigned venues)
- `read:vibes` - Read vibe definitions
- `write:vibes` - Create vibes in posts (limited to assigned venues)
- `read:venue-types` - Read venue type categories
- `read:analytics` - Access analytics (limited to assigned venues via role-based policy)
- `read:notifications` - Read notifications

**Auth0 Management API Permissions:**
- `read:users` - Read user information

## Permission Hierarchy

```
Administrator (Global Access)
├── Content Manager (All Venues)
├── Venue Owner (Assigned Venues)
│   └── Venue Manager (Assigned Venues)
└── Regular User (Public Access)
```

## Implementation Notes

### Database-Level Authorization
While Auth0 handles application-level roles and permissions, venue-specific access control is implemented through role-based policies:

- **VenueUser Table**: Links users to specific venues they can manage
- **Role-Based Policies**: Determine when to apply row-level security based on user's role
- **API Middleware**: Validates Auth0 permissions and applies venue filtering for venue-scoped roles

### Permission Checking Flow
1. **Token Validation**: Verify Auth0 JWT token signature and expiration
2. **Role Extraction**: Extract user roles and permissions from token claims
3. **Policy Evaluation**: Check if user's role requires venue-level filtering (venue-owner, venue-manager)
4. **Database Lookup**: For venue-scoped roles, filter operations to assigned venues only
5. **Permission Enforcement**: Allow or deny the operation based on combined checks

### Token Structure
Access tokens include:
```json
{
  "permissions": [
    "read:venues",
    "write:specials",
    "read:users"
  ],
  "https://pulse-api.com/roles": [
    "venue-manager"
  ]
}
```

## Security Considerations

### Principle of Least Privilege
- Each role has only the minimum permissions required for their function
- Venue-specific roles are further restricted by database-level checks
- Regular audits should be performed to ensure appropriate access levels

### Permission Boundaries
- **Global vs Venue-Specific**: Clear distinction between system-wide and venue-specific permissions
- **Read vs Write**: Separate permissions for reading and modifying data
- **Content Moderation**: Special permissions for content management and moderation

### Audit Trail
All administrative actions should be logged including:
- User role assignments and changes
- Venue user assignments
- Permission grants and revocations
- Content moderation actions

## Authorization System Architecture

The Pulse application implements a simple, elegant authorization system where each Auth0 permission automatically becomes a policy with the same name. No configuration files are needed for permission-based authorization.

### Automatic Permission Policy Creation

The system automatically creates individual policies for each Auth0 permission with the same name:

- `read:venues` permission → `read:venues` policy (automatically created)
- `write:venues` permission → `write:venues` policy (automatically created) 
- `delete:venues` permission → `delete:venues` policy (automatically created)
- `write:specials` permission → `write:specials` policy (automatically created)
- And so on for all permissions...

### Controller Usage Examples

```csharp
// Global permission-based authorization  
[Authorize(Policy = "read:venues")]
[HttpGet]
public async Task<IActionResult> GetAllVenues()
{
    // Available to any user with read:venues permission
    // No appsettings.json configuration required
}

// Another permission example
[Authorize(Policy = "write:specials")]
[HttpPost("venues/{venueId}/specials")]
public async Task<IActionResult> CreateSpecial(int venueId, SpecialCreateRequest request)
{
    // Available to any user with write:specials permission
}
```

### Role-Based Venue Scoping

For venue-specific operations, additional authorization handlers check:
1. User's role (venue-owner, venue-manager)
2. User's venue assignments in the database  
3. Apply row-level security for venue-scoped operations

### Configuration Requirements

**appsettings.json** - Only basic Auth0 configuration needed:
```json
{
  "AUTH0_AUDIENCE": "https://pulse.mirthsystems.com",
  "AUTH0_DOMAIN": "mirthsystems.us.auth0.com"
}
```

**No additional authorization configuration required** - the system automatically:
- Creates policies for each permission with the same name
- Maps Auth0 permissions directly to policy names
- Handles role-based venue scoping through authorization handlers

### Benefits of This Simple Architecture

1. **Zero Configuration**: Permission policies are created automatically - no appsettings.json config needed
2. **Direct Permission Mapping**: Auth0 permission names are used directly as policy names  
3. **Simple Controller Usage**: Use permission names directly: `[Authorize(Policy = "read:venues")]`
4. **Immediate Availability**: New Auth0 permissions are instantly usable as policies
5. **Clean and Maintainable**: No complex configuration files to manage

### Adding New Features

When adding a new feature that requires permissions:

1. **Add Permission to Auth0**: Create the permission in Auth0 dashboard (e.g., `export:data`)
2. **Use Immediately**: Apply to controller: `[Authorize(Policy = "export:data")]`
3. **Assign to Roles**: Update role assignments in Auth0 to include the new permission
4. **No Code Changes Required**: The policy is automatically available

Example:
```csharp
// New permission added to Auth0: export:data
[Authorize(Policy = "export:data")]
public async Task<IActionResult> ExportVenueData(int venueId)
{
    // Immediately usable - no configuration needed
}
```

## Future Considerations

### Planned Enhancements
- **Regional Managers**: Role for managing venues within specific geographic regions
- **Analytics Viewers**: Read-only analytics access for stakeholders  
- **API Partners**: Limited access for third-party integrations
- **Automated Moderation**: Permissions for AI-driven content moderation systems

### Scalability
- Role-based permissions can be extended without requiring Auth0 configuration changes
- New venue-specific permissions can be added through database configuration
- Permission inheritance and role hierarchies can be implemented as needed
