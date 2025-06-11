using Microsoft.AspNetCore.Identity;

namespace PulseByMirthSystems.Domain.Entities;

/// <summary>
/// Represents the many-to-many relationship between users and roles in the Pulse application.
/// This entity enables navigation between ApplicationUser and ApplicationRole entities.
/// </summary>
public class ApplicationUserRole : IdentityUserRole<long>
{
    /// <summary>
    /// Navigation property to the user in this user-role relationship.
    /// </summary>
    public virtual ApplicationUser User { get; set; } = null!;

    /// <summary>
    /// Navigation property to the role in this user-role relationship.
    /// </summary>
    public virtual ApplicationRole Role { get; set; } = null!;
}
