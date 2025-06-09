using NodaTime;

namespace Pulse.Core.Data.Entities;

/// <summary>
/// Represents an activity thread for a venue that expires after 15 minutes
/// </summary>
public class ActivityThread
{
    #region Identity and primary fields
    public long Id { get; set; }
    public long VenueId { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    #endregion

    #region Timing information
    public Instant CreatedAt { get; set; }
    public Instant ExpiresAt { get; set; }
    public bool IsActive { get; set; } = true;
    #endregion

    #region Audit information
    public required string CreatedByUserId { get; set; }
    #endregion

    #region Navigation properties
    public Venue? Venue { get; set; }
    public IList<Post> Posts { get; set; } = new List<Post>();
    #endregion
}
