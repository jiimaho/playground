using Chatty.Silo.Features.Chatroom.Grains;
using NodaTime;
using NodaTime.Extensions;

namespace Chatty.Silo.Primitives;

[Alias("ChatMessage")]
[GenerateSerializer]
public sealed class ChatMessage : ValueObject
{
    [Id(0)]
    public required Username Username { get; init; }

    [Id(1)]
    public required string Message { get; init; }

    [Id(2)]
    public required DateTimeOffset Timestamp { get; init; }

    [Id(3)]
    public required string ChatRoomId { get; init; }

    [Id(4)]
    public required ZonedDateTime TimeStampNoda { get; init; }

    [Id(5)]
    public required Guid Id { get; init; }

    public override string ToString() => $"{Timestamp:HH:mm:ss} {Username}: {Message}";

    private ChatMessage()
    {
    }

    public static ChatMessage Create(Username username, string message, string chatRoomId, DateTimeOffset? time = null)
    {
        if (string.IsNullOrWhiteSpace(message))
            throw new ArgumentException("Message cannot be empty", nameof(message));
        if (string.IsNullOrWhiteSpace(chatRoomId))
            throw new ArgumentException("ChatRoomId cannot be empty", nameof(chatRoomId));

        var timestamp = time ?? DateTimeOffset.UtcNow;
        var timeStampNoda = timestamp.ToInstant().InUtc();
        return new ChatMessage
        {
            Username = username,
            Message = message,
            Timestamp = timestamp,
            TimeStampNoda = timeStampNoda,
            ChatRoomId = chatRoomId,
            Id = Guid.CreateVersion7()
        };
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Username;
        yield return Message;
        yield return Timestamp;
        yield return TimeStampNoda;
        yield return ChatRoomId;
    }

    public ChatMessageEntity ToEntity() => new(ChatRoomId, Username.Value, Message, Timestamp);
}