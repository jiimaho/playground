using System.Collections.Immutable;
using Orleans.Concurrency;

namespace Orleans.Silo;

public interface IChatRoomObserver : IGrainObserver
{
    [OneWay]
    Task ReceiveMessage(ChatMessage message);
    
    Task UsersOnlineChanged(ImmutableArray<string> usersOnline);
}