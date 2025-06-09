using NodaTime;

namespace Pulse.Core.Data.Entities;

/// <summary>
/// Represents a user post within an activity thread
/// </summary>
public class Post
{
    #region Identity and primary fields
    public long Id { get; set; }
    public long ActivityThreadId { get; set; }
    public long UserId { get; set; }
    public required string Content { get; set; }
    #endregion

    #region Metadata
    public Instant CreatedAt { get; set; }
    public Instant? UpdatedAt { get; set; }
    public bool IsActive { get; set; } = true;
    public Instant? DeletedAt { get; set; }
    public long? DeletedByUserId { get; set; }
    #endregion

    #region Navigation properties
    public ActivityThread? ActivityThread { get; set; }
    public User? User { get; set; }
    public User? DeletedBy { get; set; }
    public IList<PostVibe> PostVibes { get; set; } = new List<PostVibe>();
    public IList<Media> MediaAttachments { get; set; } = new List<Media>();
    #endregion
}
