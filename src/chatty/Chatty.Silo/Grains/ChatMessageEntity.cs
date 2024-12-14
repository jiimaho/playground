using Chatty.Silo.Primitives;
using NodaTime;
using NodaTime.Extensions;

namespace Chatty.Silo.Grains;

public record ChatMessageEntity(
    string ChatRoomId,
    string Username,
    string Message,
    DateTimeOffset Timestamp)
{
    public ChatMessage ToDomain() => new()
    {
        Username = Primitives.Username.Create(Username),
        Message = Message,
        Timestamp = Timestamp,
        ChatRoomId = ChatRoomId,
        TimeStampNoda = Timestamp.ToInstant().InUtc()
    };
}