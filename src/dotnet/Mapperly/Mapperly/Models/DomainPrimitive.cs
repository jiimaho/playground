namespace Mapperly.Models;

public class DomainPrimitive<T> : ValueObject
{
    public T Value { get; }

    public DomainPrimitive(T value)
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }
        
        Value = value;
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value!;
    }

    public override string ToString()
    {
        return Value!.ToString()!;
    }
}