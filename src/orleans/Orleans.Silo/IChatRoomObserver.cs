using System.Collections.Immutable;
using Orleans.Concurrency;
using Orleans.Silo.Primitives;

namespace Orleans.Silo;

public interface IChatRoomObserver : IGrainObserver
{
    [OneWay]
    Task ReceiveMessage(ChatMessage message);
    
    Task UsersOnlineChanged(ImmutableArray<Username> usersOnline);
}