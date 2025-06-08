using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace Pulse.Core.Authorization;

/// <summary>
/// Authorization handler for role-based access control (RBAC)
/// </summary>
public class RBACHandler : AuthorizationHandler<RBACRequirement>
{
    private readonly ILogger<RBACHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the RBACHandler class
    /// </summary>
    /// <param name="logger">Logger instance for debugging and monitoring</param>
    public RBACHandler(ILogger<RBACHandler> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Handles the authorization requirement by checking user permissions
    /// </summary>
    /// <param name="context">The authorization context</param>
    /// <param name="requirement">The RBAC requirement to evaluate</param>
    /// <returns>A task representing the asynchronous operation</returns>
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RBACRequirement requirement)
    {
        _logger.LogDebug("Evaluating RBAC requirement for permission: {Permission}", requirement.Permission);
        
        // Check if user has the permissions claim
        if (!context.User.HasClaim(c => c.Type == "permissions"))
        {
            _logger.LogWarning("User does not have permissions claim");
            return Task.CompletedTask;
        }

        // Find the specific permission in the user's claims
        var permission = context.User.FindFirst(c => c.Type == "permissions" && c.Value == requirement.Permission);

        if (permission == null)
        {
            _logger.LogWarning("User does not have required permission: {Permission}", requirement.Permission);
            return Task.CompletedTask;
        }

        _logger.LogDebug("User has required permission: {Permission}", requirement.Permission);
        context.Succeed(requirement);

        return Task.CompletedTask;
    }
}
