using Chatty.Silo.Primitives;
using NodaTime;

namespace Chatty.Silo.Grains;

public record ChatMessageEntity(
    string ChatRoomId,
    string Username,
    string Message,
    DateTimeOffset Timestamp)
{
    public ChatMessage ToPrimitive() => new()
    {
        ChatRoomId = ChatRoomId,
        Username = new Username(Username),
        Message = Message,
        Timestamp = Timestamp,
        TimeStampNoda = ZonedDateTime.FromDateTimeOffset(Timestamp)
    };
}