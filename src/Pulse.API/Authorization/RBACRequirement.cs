using Microsoft.AspNetCore.Authorization;

namespace Pulse.API.Requirement;

class RBACRequirement : IAuthorizationRequirement
{
    public string Permission { get; }

    public RBACRequirement(string permission)
    {
        Permission = permission ?? throw new ArgumentNullException(nameof(permission));
    }
}
