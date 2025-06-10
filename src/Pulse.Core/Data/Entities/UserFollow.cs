using NodaTime;
using Pulse.Core.Data.Enums;

namespace Pulse.Core.Data.Entities;

/// <summary>
/// Represents a user's follow relationship with tags or venues for notifications
/// </summary>
public class UserFollow
{
    #region Identity and primary fields
    public long Id { get; set; }
    public long UserId { get; set; }
    public FollowType FollowType { get; set; }
    #endregion

    #region Follow target (one of these will be set)
    public long? TagId { get; set; }
    public long? VenueId { get; set; }
    #endregion

    #region Follow settings
    public bool IsActive { get; set; } = true;
    public Instant CreatedAt { get; set; }
    public Instant? UnfollowedAt { get; set; }
    #endregion

    #region Notification preferences
    public bool NotifyNewSpecials { get; set; } = true;
    public bool NotifySpecialReminders { get; set; } = true;
    public bool NotifyVenueActivity { get; set; } = false;
    #endregion

    #region Navigation properties
    public User? User { get; set; }
    public Tag? Tag { get; set; }
    public Venue? Venue { get; set; }
    #endregion
}
