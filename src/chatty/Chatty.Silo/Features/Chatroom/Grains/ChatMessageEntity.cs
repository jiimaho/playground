using Chatty.Silo.Primitives;

namespace Chatty.Silo.Features.Chatroom.Grains;

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