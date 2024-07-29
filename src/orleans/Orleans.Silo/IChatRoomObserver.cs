namespace Orleans.Silo;

public interface IChatRoomObserver : IGrainObserver
{
    Task ReceiveMessage(ChatMessage message);
}