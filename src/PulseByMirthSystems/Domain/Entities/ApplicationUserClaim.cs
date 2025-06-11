using Microsoft.AspNetCore.Identity;

namespace PulseByMirthSystems.Domain.Entities;

/// <summary>
/// Represents a claim associated with a user in the Pulse application.
/// Claims are key-value pairs that represent permissions or other user-specific data.
/// </summary>
public class ApplicationUserClaim : IdentityUserClaim<long>
{
    /// <summary>
    /// Navigation property to the user that owns this claim.
    /// </summary>
    public virtual ApplicationUser User { get; set; } = null!;
}
