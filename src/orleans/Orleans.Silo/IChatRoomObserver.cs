using Orleans.Concurrency;

namespace Orleans.Silo;

public interface IChatRoomObserver : IGrainObserver
{
    [OneWay]
    Task ReceiveMessage(ChatMessage message);
}