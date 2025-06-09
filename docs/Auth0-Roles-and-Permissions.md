# Pulse Auth0 Roles and Permissions Documentation

## Overview
This document provides a comprehensive reference for all Auth0 roles and permissions used in the Pulse application. The system uses a combination of custom Pulse API permissions and Auth0 Management API permissions to provide granular access control.

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
| `read:venues` | Read All Venues | Query and view all venues (global access) |
| `write:venues` | Write All Venues | Create and update any venue information (global access) |
| `delete:venues` | Delete All Venues | Delete any venue from the system (global access) |
| `read:assigned-venues` | Read Assigned Venues | Query and view venues assigned to the current user |
| `write:assigned-venues` | Write Assigned Venues | Update venue information for assigned venues only |
| `delete:assigned-venues` | Delete Assigned Venues | Delete assigned venues only |

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
- `read:analytics` - Access all venue analytics
- `read:analytics-global` - Access global platform analytics
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

- `read:assigned-venues` - Query and view assigned venues (for backoffice access)
- `write:assigned-venues` - Update venue information (limited to assigned venues via database)
- `delete:assigned-venues` - Delete assigned venues (limited to assigned venues via database)
- `write:specials` - Create and update specials (limited to assigned venues via database)
- `delete:specials` - Delete specials (limited to assigned venues via database)
- `read:content` - Read content (limited to assigned venues via database)
- `write:content` - Create content (limited to assigned venues via database)
- `read:analytics` - Access analytics (limited to assigned venues via database)
- `read:venue-users` - View venue user assignments (limited to assigned venues via database)
- `write:venue-users` - Manage venue user assignments (limited to assigned venues via database)
- `delete:venue-users` - Remove users from venues (limited to assigned venues via database)

**Auth0 Management API Permissions:**
- `read:users` - Read user information
- `update:users` - Update user profiles

### Venue Manager
- **Role Name**: `venue-manager`
- **Description**: Manage specials and content for assigned venues
- **Use Case**: Staff members who manage day-to-day operations for specific venues

#### Assigned Permissions

**Pulse API Permissions:**

- `read:assigned-venues` - Query and view assigned venues (for backoffice access)
- `write:specials` - Create and update specials (limited to assigned venues via database)
- `delete:specials` - Delete specials (limited to assigned venues via database)
- `read:content` - Read content (limited to assigned venues via database)
- `write:content` - Create content (limited to assigned venues via database)
- `read:analytics` - Access analytics (limited to assigned venues via database)

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
While Auth0 handles application-level roles and permissions, venue-specific access control is implemented at the database level:

- **VenueUser Table**: Links users to specific venues they can manage
- **VenueUserToPermissionLink Table**: Defines specific permissions for each user-venue relationship
- **API Middleware**: Validates both Auth0 permissions and database-level venue assignments

### Permission Checking Flow
1. **Token Validation**: Verify Auth0 JWT token signature and expiration
2. **Role Extraction**: Extract user roles and permissions from token claims
3. **Database Lookup**: For venue-specific operations, verify user has access to the specific venue
4. **Permission Enforcement**: Allow or deny the operation based on combined checks

### Token Structure
Access tokens include:
```json
{
  "permissions": [
    "read:assigned-venues",
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
