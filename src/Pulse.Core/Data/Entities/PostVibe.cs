namespace Pulse.Core.Data.Entities;

/// <summary>
/// Many-to-many relationship between Posts and Vibes
/// </summary>
public class PostVibe
{
    #region Identity and primary fields
    public long PostId { get; set; }
    public long VibeId { get; set; }
    #endregion

    #region Navigation properties
    public Post? Post { get; set; }
    public Vibe? Vibe { get; set; }
    #endregion
}
