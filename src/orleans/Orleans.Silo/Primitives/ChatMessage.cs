using NodaTime;
using Orleans.Silo.Grains;

namespace Orleans.Silo.Primitives;

// TOOD: Make immutable ❤️
// Use complex types, and codecs
[Alias("ChatMessage")]
[GenerateSerializer]
public class ChatMessage : ValueObject
{
    [Id(0)]
    public Username Username { get; init; }

    [Id(1)]
    public string Message { get; init; }

    [Id(2)]
    public DateTimeOffset Timestamp { get; init; }

    [Id(3)]
    public string ChatRoomId { get; init; }

    [Id(4)]
    public ZonedDateTime TimeStampNoda { get; init; } 

    public override string ToString() => $"{Timestamp:HH:mm:ss} {Username}: {Message}";
    
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