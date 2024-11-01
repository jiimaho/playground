namespace Orleans.Silo.Primitives;

[Alias("ChatMessage")]
[GenerateSerializer]
public class ChatMessage : ValueObject
{
    [Id(0)]
    public Username Username { get; set; }
    
    [Id(1)]
    public string Message { get; set; }
    
    [Id(2)]
    public DateTimeOffset Timestamp { get; } = DateTimeOffset.Now;

    [Id(3)]
    public string ChatRoomId { get; set; } 

    // ReSharper disable once ConvertToPrimaryConstructor
    public ChatMessage(Username username, string message, string chatRoomId)
    {
        Username = username;
        Message = message;
        ChatRoomId = chatRoomId;
    }

    public override string ToString() => $"{Timestamp:HH:mm:ss} {Username}: {Message}";
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Username;
        yield return Message;
        yield return Timestamp;
        yield return ChatRoomId;
    }
}