using Chatty.Silo.Primitives;

namespace Chatty.Web.Notifications;

public interface INotificationService
{
    event Action<Username> UserOnline;
    
    Task SendNotification(Username username);
}

public class NotificationService : INotificationService
{

    public NotificationService()
    {
    }

    public event Action<Username>? UserOnline;

    public Task SendNotification(Username username)
    {
        return Task.CompletedTask;
    }
}