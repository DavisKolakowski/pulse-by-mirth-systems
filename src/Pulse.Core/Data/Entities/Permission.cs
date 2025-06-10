using NodaTime;

namespace Pulse.Core.Data.Entities;

/// <summary>
/// Represents a granular permission in the system
/// </summary>
public class Permission
{
    #region Identity and primary fields
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string DisplayName { get; set; }
    public string? Description { get; set; }
    #endregion

    #region Categorization
    public required string Category { get; set; }  // "Venue", "Special", "Content", "Analytics", etc.
    public required string Action { get; set; }    // "read", "write", "delete", "moderate", etc.
    public required string Resource { get; set; }  // "venues", "specials", "content", etc.
    #endregion

    #region Metadata properties
    public bool IsActive { get; set; } = true;
    public int SortOrder { get; set; }
    public Instant CreatedAt { get; set; }
    #endregion

    #region Navigation properties
    public IList<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    #endregion
}
