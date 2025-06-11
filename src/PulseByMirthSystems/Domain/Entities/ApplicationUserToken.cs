using Microsoft.AspNetCore.Identity;

namespace PulseByMirthSystems.Domain.Entities;

/// <summary>
/// Represents an authentication token for a user in the Pulse application.
/// Tokens are used for features like password reset, email confirmation, two-factor authentication, etc.
/// </summary>
public class ApplicationUserToken : IdentityUserToken<long>
{
    /// <summary>
    /// Navigation property to the user that owns this token.
    /// </summary>
    public virtual ApplicationUser User { get; set; } = null!;
}
