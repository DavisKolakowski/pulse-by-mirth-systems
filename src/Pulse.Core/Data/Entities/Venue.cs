using NodaTime;
using NetTopologySuite.Geometries;

namespace Pulse.Core.Data.Entities;

/// <summary>
/// Represents a venue in the Pulse application
/// </summary>
public class Venue
{
    #region Identity and primary fields
    public long Id { get; set; }
    public int CategoryId { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    #endregion

    #region Contact information
    public string? PhoneNumber { get; set; }
    public string? Website { get; set; }
    public string? Email { get; set; }
    public string? ProfileImage { get; set; }
    #endregion

    #region Address fields
    public required string StreetAddress { get; set; }
    public string? SecondaryAddress { get; set; }
    public required string Locality { get; set; }
    public required string Region { get; set; }
    public required string PostalCode { get; set; }
    public required string Country { get; set; }
    public Point? Location { get; set; }
    #endregion

    #region Audit information
    public Instant CreatedAt { get; set; }
    public required string CreatedByUserId { get; set; }
    public Instant? UpdatedAt { get; set; }
    public string? UpdatedByUserId { get; set; }
    public bool IsActive { get; set; } = true;
    public Instant? DeactivatedAt { get; set; }
    public string? DeactivatedByUserId { get; set; }
    #endregion

    #region Navigation properties
    public VenueCategory? Category { get; set; }
    public IList<BusinessHours> BusinessHours { get; set; } = new List<BusinessHours>();
    public IList<Special> Specials { get; set; } = new List<Special>();
    public IList<VenueRole> VenueRoles { get; set; } = new List<VenueRole>();
    public IList<ActivityThread> ActivityThreads { get; set; } = new List<ActivityThread>();
    public IList<Media> MediaFiles { get; set; } = new List<Media>();
    public IList<UserFollow> Followers { get; set; } = new List<UserFollow>();
    public IList<Notification> RelatedNotifications { get; set; } = new List<Notification>();
    #endregion
}
