namespace Orleans.Silo;

public record ChatRoomState
{
    public List<ChatMessage> History { get; } = [];
};