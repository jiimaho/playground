namespace Orleans.Silo;

[Alias("ChatMessage")]
[GenerateSerializer]
public record ChatMessage(string User, string Message)
{
    [Id(0)]
    public DateTimeOffset Timestamp { get; } = DateTimeOffset.Now;

    public override string ToString() => $"{Timestamp:HH:mm:ss} {User}: {Message}";
}