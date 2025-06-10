namespace Pulse.Core.Authorization;

/// <summary>
/// Constants for permission actions used throughout the system
/// </summary>
public static class PermissionActions
{
    public const string Read = "read";
    public const string Write = "write";
    public const string Delete = "delete";
    public const string Moderate = "moderate";
    public const string Upload = "upload";
    public const string Manage = "manage";
    public const string Admin = "admin";
    public const string Config = "config";
}

/// <summary>
/// Constants for permission resources used throughout the system
/// </summary>
public static class PermissionResources
{
    public const string Venues = "venues";
    public const string Specials = "specials";
    public const string Content = "content";
    public const string Posts = "posts";
    public const string Media = "media";
    public const string Tags = "tags";
    public const string Vibes = "vibes";
    public const string VenueCategories = "venue-categories";
    public const string Analytics = "analytics";
    public const string AnalyticsGlobal = "analytics-global";
    public const string Notifications = "notifications";
    public const string Follows = "follows";
    public const string VenueUsers = "venue-users";
    public const string System = "system";
}

/// <summary>
/// Constants for permission categories used throughout the system
/// </summary>
public static class PermissionCategories
{
    public const string Venue = "Venue";
    public const string Special = "Special";
    public const string Content = "Content";
    public const string Post = "Post";
    public const string Media = "Media";
    public const string Tag = "Tag";
    public const string Vibe = "Vibe";
    public const string VenueCategory = "VenueCategory";
    public const string Analytics = "Analytics";
    public const string Notification = "Notification";
    public const string Follow = "Follow";
    public const string VenueUser = "VenueUser";
    public const string System = "System";
}
