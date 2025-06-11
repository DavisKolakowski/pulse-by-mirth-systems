using Microsoft.AspNetCore.Identity;

namespace PulseByMirthSystems.Domain.Entities;

/// <summary>
/// Represents a role in the Pulse application, extending ASP.NET Core Identity's IdentityRole
/// with additional navigation properties for relationship management.
/// </summary>
public class ApplicationRole : IdentityRole<long>
{
    /// <summary>
    /// Collection of user-role assignments for this role.
    /// Represents the many-to-many relationship between users and roles.
    /// </summary>
    public virtual ICollection<ApplicationUserRole> UserRoles { get; set; } = new List<ApplicationUserRole>();

    /// <summary>
    /// Collection of claims associated with this role.
    /// Role claims represent permissions that are granted to all users assigned to this role.
    /// </summary>
    public virtual ICollection<ApplicationRoleClaim> RoleClaims { get; set; } = new List<ApplicationRoleClaim>();
}
