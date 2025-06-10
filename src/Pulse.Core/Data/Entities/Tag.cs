using NodaTime;

namespace Pulse.Core.Data.Entities;

/// <summary>
/// Represents a tag that can be applied to specials for discovery
/// </summary>
public class Tag
{
    #region Identity and primary fields
    public long Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    #endregion

    #region Metadata properties
    public string? Color { get; set; }
    public string? Icon { get; set; }
    public bool IsActive { get; set; } = true;
    public int UsageCount { get; set; } = 0;
    #endregion

    #region Audit information
    public Instant CreatedAt { get; set; }
    public required string CreatedByUserId { get; set; }
    public Instant? UpdatedAt { get; set; }
    public string? UpdatedByUserId { get; set; }
    public Instant? DeactivatedAt { get; set; }
    public string? DeactivatedByUserId { get; set; }
    #endregion

    #region Navigation properties
    public IList<SpecialTag> SpecialTags { get; set; } = new List<SpecialTag>();
    public IList<UserFollow> Followers { get; set; } = new List<UserFollow>();
    #endregion
}
