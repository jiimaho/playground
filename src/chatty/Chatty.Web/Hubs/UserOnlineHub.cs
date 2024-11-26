using Microsoft.AspNetCore.SignalR;

namespace Chatty.Web.Hubs;

public class UserOnlineHub : Hub<IUserOnlineHub>
{
    
}

public interface IUserOnlineHub
{
    Task UserOnline(string username);
}