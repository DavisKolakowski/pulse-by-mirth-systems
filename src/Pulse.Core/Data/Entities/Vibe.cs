namespace Pulse.Core.Data.Entities;

/// <summary>
/// Represents a vibe/atmosphere descriptor that can be applied to posts
/// </summary>
public class Vibe
{
    #region Identity and primary fields
    public long Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    #endregion

    #region Metadata properties
    public string? Color { get; set; }
    public string? Icon { get; set; }
    public bool IsSystemVibe { get; set; } = false;
    public bool IsActive { get; set; } = true;
    public int UsageCount { get; set; } = 0;
    #endregion

    #region Navigation properties
    public IList<PostVibe> PostVibes { get; set; } = new List<PostVibe>();
    #endregion
}
