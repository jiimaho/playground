namespace Orleans.Silo.Primitives;

[GenerateSerializer]
public abstract class ValueObject
{
    public override bool Equals(object? obj)
    {
        if (obj == null || obj.GetType() != GetType())
        {
            return false;
        }
        
        var components = GetEqualityComponents().ToList();
        var components2 = ((ValueObject)obj).GetEqualityComponents().ToList();

        for (var i = 0; i < components.Count; i++)
        {
            var c1 = components[i];
            var c2 = components2[i];
            
            if (c1 != c2)
            {
                return false;
            }
        }

        return true;
    }

    public override int GetHashCode()
    {
        return GetEqualityComponents()
            .Aggregate(1, (current, obj) => current * 23 + (obj?.GetHashCode() ?? 0));
    }

    public virtual IEnumerable<object> GetEqualityComponents()
    {
        return new List<object>();
    }
}