using NodaTime;

namespace Pulse.Core.Data.Entities;

/// <summary>
/// Represents a user in the Pulse application, linked to Auth0
/// </summary>
public class User
{
    #region Identity and primary fields
    public long Id { get; set; }
    public required string Auth0UserId { get; set; }
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
    public string? DeactivatedByUserId { get; set; }
    #endregion

    #region Navigation properties
    public IList<VenueRole> VenueRoles { get; set; } = new List<VenueRole>();
    public IList<Post> Posts { get; set; } = new List<Post>();
    public IList<UserFollow> Follows { get; set; } = new List<UserFollow>();
    public IList<Notification> SentNotifications { get; set; } = new List<Notification>();
    public IList<Notification> ReceivedNotifications { get; set; } = new List<Notification>();
    #endregion
}
