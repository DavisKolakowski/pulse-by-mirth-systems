using NodaTime;

namespace Pulse.Core.Data.Entities;

/// <summary>
/// Represents an application role (Administrator, ContentManager, VenueOwner, VenueManager)
/// </summary>
public class Role
{
    #region Identity and primary fields
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string DisplayName { get; set; }
    public string? Description { get; set; }
    #endregion

    #region Metadata properties
    public bool IsActive { get; set; } = true;
    public int SortOrder { get; set; }
    public Instant CreatedAt { get; set; }
    #endregion

    #region Navigation properties
    public IList<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    public IList<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public IList<VenueRole> VenueRoles { get; set; } = new List<VenueRole>();
    #endregion
}
