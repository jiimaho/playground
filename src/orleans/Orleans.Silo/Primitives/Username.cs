namespace Orleans.Silo.Primitives;

[GenerateSerializer]
public class Username : ValueObject
{
    [Id(0)]
    public string Value { get; }

    public Username(string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        Value = value;
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}