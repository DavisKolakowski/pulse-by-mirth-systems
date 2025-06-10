using NodaTime;
using Pulse.Core.Data.Enums;

namespace Pulse.Core.Data.Entities;

/// <summary>
/// Represents media content (photos/videos) attached to venues or posts
/// </summary>
public class Media
{
    #region Identity and primary fields
    public long Id { get; set; }
    public required string FileName { get; set; }
    public required string FilePath { get; set; }
    public required string ContentType { get; set; }
    public long FileSize { get; set; }
    #endregion

    #region Media properties
    public int? Width { get; set; }
    public int? Height { get; set; }
    public int? DurationSeconds { get; set; }
    public string? AltText { get; set; }
    #endregion

    #region Association information
    public long? VenueId { get; set; }
    public long? PostId { get; set; }
    public MediaType MediaType { get; set; }
    #endregion

    #region Audit information
    public Instant CreatedAt { get; set; }
    public required string CreatedByUserId { get; set; }
    public bool IsActive { get; set; } = true;
    public Instant? DeletedAt { get; set; }
    public string? DeletedByUserId { get; set; }
    #endregion

    #region Navigation properties
    public Venue? Venue { get; set; }
    public Post? Post { get; set; }
    #endregion
}
