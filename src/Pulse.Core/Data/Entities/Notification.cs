using NodaTime;

namespace Pulse.Core.Data.Entities;

/// <summary>
/// Represents a notification sent to a user
/// </summary>
public class Notification
{
    #region Identity and primary fields
    public long Id { get; set; }
    public long RecipientUserId { get; set; }
    public long? SenderUserId { get; set; }
    public required string Title { get; set; }
    public required string Message { get; set; }
    #endregion

    #region Notification properties
    public NotificationType Type { get; set; }
    public string? ActionUrl { get; set; }
    public string? ImageUrl { get; set; }
    #endregion

    #region Status information
    public Instant CreatedAt { get; set; }
    public bool IsRead { get; set; } = false;
    public Instant? ReadAt { get; set; }
    public bool IsActive { get; set; } = true;
    #endregion

    #region Related entity information
    public long? RelatedVenueId { get; set; }
    public long? RelatedSpecialId { get; set; }
    public long? RelatedTagId { get; set; }
    #endregion

    #region Navigation properties
    public User? Recipient { get; set; }
    public User? Sender { get; set; }
    public Venue? RelatedVenue { get; set; }
    public Special? RelatedSpecial { get; set; }
    public Tag? RelatedTag { get; set; }
    #endregion
}

/// <summary>
/// Enumeration of notification types
/// </summary>
public enum NotificationType
{
    NewSpecial = 1,
    SpecialReminder = 2,
    VenueActivity = 3,
    TagActivity = 4,
    SystemAlert = 5,
    Welcome = 6
}
