using NodaTime;

namespace Pulse.Core.Data.Entities;

/// <summary>
/// Represents a user's role assignment to a specific venue
/// </summary>
public class VenueRole
{
    #region Identity and primary fields
    public long Id { get; set; }
    public long UserId { get; set; }
    public long VenueId { get; set; }
    public required string RoleName { get; set; }
    #endregion

    #region Assignment information
    public Instant AssignedAt { get; set; }
    public long? AssignedByUserId { get; set; }
    public bool IsActive { get; set; } = true;
    public Instant? DeactivatedAt { get; set; }
    public long? DeactivatedByUserId { get; set; }
    #endregion

    #region Navigation properties
    public User? User { get; set; }
    public Venue? Venue { get; set; }
    public User? AssignedBy { get; set; }
    public User? DeactivatedBy { get; set; }
    #endregion
}
