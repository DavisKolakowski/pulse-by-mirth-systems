namespace Pulse.Core.Data.Entities;

/// <summary>
/// Many-to-many relationship between Roles and Permissions
/// </summary>
public class RolePermission
{
    #region Identity and primary fields
    public int RoleId { get; set; }
    public int PermissionId { get; set; }
    #endregion

    #region Navigation properties
    public Role? Role { get; set; }
    public Permission? Permission { get; set; }
    #endregion
}
