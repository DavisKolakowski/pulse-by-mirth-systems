using Microsoft.AspNetCore.Identity;

namespace PulseByMirthSystems.Domain.Entities;

/// <summary>
/// Represents a user in the Pulse application, extending ASP.NET Core Identity's IdentityUser
/// with additional navigation properties for full relationship management.
/// </summary>
public class ApplicationUser : IdentityUser<long>
{
    /// <summary>
    /// Collection of claims associated with this user.
    /// Claims represent permissions and other user-specific data.
    /// </summary>
    public virtual ICollection<ApplicationUserClaim> Claims { get; set; } = new List<ApplicationUserClaim>();

    /// <summary>
    /// Collection of external login providers associated with this user.
    /// Supports authentication through providers like Google, Facebook, etc.
    /// </summary>
    public virtual ICollection<ApplicationUserLogin> Logins { get; set; } = new List<ApplicationUserLogin>();

    /// <summary>
    /// Collection of authentication tokens for this user.
    /// Used for features like password reset, email confirmation, etc.
    /// </summary>
    public virtual ICollection<ApplicationUserToken> Tokens { get; set; } = new List<ApplicationUserToken>();

    /// <summary>
    /// Collection of role assignments for this user.
    /// Represents the many-to-many relationship between users and roles.
    /// </summary>
    public virtual ICollection<ApplicationUserRole> UserRoles { get; set; } = new List<ApplicationUserRole>();
}
