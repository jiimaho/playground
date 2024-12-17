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
    public ChatMessage ToDomain() => ChatMessage.Create(
        Primitives.Username.Create(Username),
        Message,
        ChatRoomId,
        Timestamp);
}