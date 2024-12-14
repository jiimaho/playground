namespace Chatty.Silo.Primitives;

[Alias("Username")]
[GenerateSerializer]
public sealed class Username : ValueObject
{
    [Id(0)]
    public required string Value { get; init; }

    // For orleans to be able to deserialize
    // ReSharper disable once EmptyConstructor
    public Username() { }
    
    public static Username Create(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("Username cannot be empty", nameof(username));
        
        return new Username
        {
            Value = username
        };
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}