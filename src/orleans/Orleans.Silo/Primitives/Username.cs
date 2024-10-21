namespace Orleans.Silo.Primitives;

[GenerateSerializer]
public class Username : ValueObject
{
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