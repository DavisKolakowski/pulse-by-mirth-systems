using Pulse.Core.Models;

namespace Pulse.Core.Services;

/// <summary>
/// Service for managing message operations
/// </summary>
public class MessageService : IMessageService
{
    /// <summary>
    /// Gets an admin message that requires admin permissions
    /// </summary>
    /// <returns>An admin message</returns>
    public Message GetAdminMessage()
    {
        return new Message { Text = "This is an admin message." };
    }

    /// <summary>
    /// Gets a protected message that requires authentication
    /// </summary>
    /// <returns>A protected message</returns>
    public Message GetProtectedMessage()
    {
        return new Message { Text = "This is a protected message." };
    }

    /// <summary>
    /// Gets a public message that doesn't require authentication
    /// </summary>
    /// <returns>A public message</returns>
    public Message GetPublicMessage()
    {
        return new Message { Text = "This is a public message." };
    }
}
