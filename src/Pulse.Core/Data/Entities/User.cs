using NodaTime;

namespace Pulse.Core.Data.Entities;

/// <summary>
/// Represents a user in the Pulse application, provider-agnostic
/// </summary>
public class User
{
    #region Identity and primary fields
    public long Id { get; set; }
    public required string ProviderId { get; set; }  // Auth0 user ID
    public required string Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? DisplayName { get; set; }
    public string? ProfilePicture { get; set; }
    #endregion
    #region Contact information
    public string? PhoneNumber { get; set; }
    #endregion

    #region Audit information
    public Instant CreatedAt { get; set; }
    public Instant? UpdatedAt { get; set; }
    public Instant? LastLoginAt { get; set; }
    public bool IsActive { get; set; } = true;
    public Instant? DeactivatedAt { get; set; }
    public long? DeactivatedByUserId { get; set; }
    #endregion

    #region Navigation properties
    public IList<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public IList<VenueRole> VenueRoles { get; set; } = new List<VenueRole>();
    public IList<Post> Posts { get; set; } = new List<Post>();
    public IList<UserFollow> Follows { get; set; } = new List<UserFollow>();
    public IList<Notification> SentNotifications { get; set; } = new List<Notification>();
    public IList<Notification> ReceivedNotifications { get; set; } = new List<Notification>();
    #endregion
}
