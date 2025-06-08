using Pulse.API.Requirement;
using Microsoft.AspNetCore.Authorization;

namespace Pulse.API.Authorization;

class RBACHandler : AuthorizationHandler<RBACRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RBACRequirement requirement)
    {
        if (!context.User.HasClaim(c => c.Type == "permissions"))
        {
            return Task.CompletedTask;
        }

        var permission = context.User.FindFirst(c => c.Type == "permissions" && c.Value == requirement.Permission);

        if (permission == null)
        {
            return Task.CompletedTask;
        }

        context.Succeed(requirement);

        return Task.CompletedTask;
    }
}
