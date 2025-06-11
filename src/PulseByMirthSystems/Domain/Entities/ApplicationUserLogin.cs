using Microsoft.AspNetCore.Identity;

namespace PulseByMirthSystems.Domain.Entities;

/// <summary>
/// Represents an external login provider association for a user in the Pulse application.
/// This enables users to authenticate using external providers like Google, Facebook, etc.
/// </summary>
public class ApplicationUserLogin : IdentityUserLogin<long>
{
    /// <summary>
    /// Navigation property to the user that owns this external login.
    /// </summary>
    public virtual ApplicationUser User { get; set; } = null!;
}
