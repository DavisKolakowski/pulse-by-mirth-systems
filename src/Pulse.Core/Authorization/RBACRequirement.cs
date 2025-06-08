using Microsoft.AspNetCore.Authorization;

namespace Pulse.Core.Authorization;

/// <summary>
/// Authorization requirement for role-based access control (RBAC)
/// </summary>
public class RBACRequirement : IAuthorizationRequirement
{
    /// <summary>
    /// The required permission for this authorization requirement
    /// </summary>
    public string Permission { get; }

    /// <summary>
    /// Initializes a new instance of the RBACRequirement class
    /// </summary>
    /// <param name="permission">The required permission</param>
    /// <exception cref="ArgumentNullException">Thrown when permission is null</exception>
    public RBACRequirement(string permission)
    {
        Permission = permission ?? throw new ArgumentNullException(nameof(permission));
    }
}
