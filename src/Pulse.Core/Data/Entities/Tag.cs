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
    public bool IsSystemTag { get; set; } = false;
    public bool IsActive { get; set; } = true;
    public int UsageCount { get; set; } = 0;
    #endregion

    #region Navigation properties
    public IList<SpecialTag> SpecialTags { get; set; } = new List<SpecialTag>();
    public IList<UserFollow> Followers { get; set; } = new List<UserFollow>();
    #endregion
}
