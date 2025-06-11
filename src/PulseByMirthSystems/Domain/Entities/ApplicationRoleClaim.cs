using Microsoft.AspNetCore.Identity;

namespace PulseByMirthSystems.Domain.Entities;

/// <summary>
/// Represents a claim associated with a role in the Pulse application.
/// Role claims represent permissions that are automatically granted to all users assigned to the role.
/// </summary>
public class ApplicationRoleClaim : IdentityRoleClaim<long>
{
    /// <summary>
    /// Navigation property to the role that owns this claim.
    /// </summary>
    public virtual ApplicationRole Role { get; set; } = null!;
}
