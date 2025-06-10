using NodaTime;

namespace Pulse.Core.Data.Entities;

/// <summary>
/// Represents a global role assignment to a user
/// </summary>
public class UserRole
{
    #region Identity and primary fields
    public long Id { get; set; }
    public long UserId { get; set; }
    public int RoleId { get; set; }
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
    public Role? Role { get; set; }
    public User? AssignedBy { get; set; }
    public User? DeactivatedBy { get; set; }
    #endregion
}
