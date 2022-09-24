namespace Todo.Domain;

public abstract class ValueObject
{
    protected abstract IEnumerable<object> GetEqualityComponents();
    
    public static bool operator ==(ValueObject left, ValueObject right)
    {
        if (left is null && right is null)
            return true;

        if (left is null || right is null)
            return false;

        return left.Equals(right);
    }
    
    public static bool operator !=(ValueObject left, ValueObject right) => !(left == right);

    public override bool Equals(object? other)
    {
        if (other == null)
        {
            return false;
        }
        
        if (GetType() != other.GetType())
        {
            return false;
        }

        var comp = GetEqualityComponents();
        var comp2 = ((ValueObject)other).GetEqualityComponents();
        
        return comp.SequenceEqual(comp2);
    }

    public override int GetHashCode()
    {
        return GetEqualityComponents()
            .Select(x => x != null ? x.GetHashCode() : 0)
            .Aggregate((x, y) => x ^ y);
    }

    public static implicit operator string?(ValueObject valueObject) => valueObject.ToString();
}