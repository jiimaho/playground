using Chatty.Silo.Primitives;
using Microsoft.AspNetCore.SignalR;

namespace Chatty.Web.Notifications;

public class UserOnlineHub : Hub
{
    public async Task UserOnline(Username username)
    {
        await Clients.All.SendAsync("UserOnline", username);
    }
}