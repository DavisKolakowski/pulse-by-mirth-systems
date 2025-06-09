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
- **Permission**: `read:assigned-venues`
  - **Description**: Query and view venues assigned to the current user
- **Permission**: `read:venues`
  - **Description**: Query and view all venues (global access)
- **Permission**: `write:venues`
  - **Description**: Create and update venue information
- **Permission**: `write:assigned-venues`
  - **Description**: Update venue information for assigned venues only
- **Permission**: `delete:venues`
  - **Description**: Delete venues from the system
- **Permission**: `delete:assigned-venues`
  - **Description**: Delete assigned venues only

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

- `write:venues`
- `delete:venues`
- `write:specials`
- `delete:specials`
- `read:content`
- `write:content`
- `delete:content`
- `moderate:content`
- `read:analytics`
- `read:analytics-global`
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

- `write:venues`
- `write:specials`
- `read:content`
- `write:content`
- `delete:content`
- `moderate:content`
- `read:analytics`

**From Auth0 Management API:**
- `read:users`
- `update:users`

### Venue Owner Role Permissions

**From Pulse API:**

- `read:assigned-venues`
- `write:assigned-venues`
- `delete:assigned-venues`
- `write:specials`
- `delete:specials`
- `read:content`
- `write:content`
- `read:analytics`
- `read:venue-users`
- `write:venue-users`
- `delete:venue-users`

**From Auth0 Management API:**
- `read:users`
- `update:users`

### Venue Manager Role Permissions

**From Pulse API:**

- `read:assigned-venues`
- `write:specials`
- `delete:specials`
- `read:content`
- `write:content`
- `read:analytics`

**From Auth0 Management API:**
- `read:users`

## Step 6: Configure API Authorization

1. **Navigate to your Pulse API**:
   - Go to "APIs" > "Pulse API"
   - Click on the "Settings" tab

2. **Enable RBAC**:
   - Toggle "Enable RBAC" to ON
   - Toggle "Add Permissions in the Access Token" to ON

3. **Configure Token Settings**:
   - Set Token Expiration as needed (recommend 24 hours for access tokens)
   - Configure refresh token settings if using refresh tokens

## Step 7: Update Application Settings

1. **Navigate to Applications**:
   - Go to "Applications" in the Auth0 Dashboard
   - Select your Pulse application

2. **Configure Application**:
   - Ensure the application type is set correctly (SPA for React app, Regular Web App for backend)
   - Add your callback URLs, logout URLs, and allowed origins
   - In the "APIs" tab, authorize your application to access both:
     - The Pulse API
     - The Auth0 Management API (with appropriate scopes)

## Step 8: Testing the Setup

1. **Test Authentication**:
   - Ensure users can log in and receive tokens
   - Verify that tokens contain the correct permissions

2. **Test Authorization**:
   - Create test users with different roles
   - Verify that each role has access to appropriate endpoints
   - Test that venue-specific permissions work correctly

## Important Notes

- **Venue-Specific Authorization**: While Auth0 handles application-level roles, venue-specific permissions (which venues a user can manage) should be handled in your database
- **Token Validation**: Your API should validate both the token signature and the permissions within the token
- **Permission Enforcement**: Implement middleware in your .NET API to check permissions on each endpoint
- **User Assignment**: Venue assignments should be managed through your application's admin interface, not directly in Auth0

## Security Considerations

1. **Principle of Least Privilege**: Only assign the minimum permissions necessary for each role
2. **Regular Audits**: Periodically review user roles and permissions
3. **Token Security**: Implement proper token storage and handling in your frontend
4. **API Security**: Use HTTPS and implement rate limiting
5. **Logging**: Log all administrative actions for audit purposes
