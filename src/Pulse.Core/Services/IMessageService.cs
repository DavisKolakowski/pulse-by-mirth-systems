using Pulse.Core.Models;

namespace Pulse.Core.Services;

/// <summary>
/// Interface for message service operations
/// </summary>
public interface IMessageService
{
    /// <summary>
    /// Gets a public message that doesn't require authentication
    /// </summary>
    /// <returns>A public message</returns>
    Message GetPublicMessage();
    
    /// <summary>
    /// Gets a protected message that requires authentication
    /// </summary>
    /// <returns>A protected message</returns>
    Message GetProtectedMessage();
    
    /// <summary>
    /// Gets an admin message that requires admin permissions
    /// </summary>
    /// <returns>An admin message</returns>
    Message GetAdminMessage();
}
