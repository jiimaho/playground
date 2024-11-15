namespace Orleans.Silo.Primitives;

[Alias("ValueObject")]
[GenerateSerializer]
public abstract class ValueObject
{
    public static bool operator ==(ValueObject obj1, ValueObject obj2)
    {
        return obj1.Equals(obj2);
    }
    
    public static bool operator !=(ValueObject obj1, ValueObject obj2)
    {
        return !obj1.Equals(obj2);
    }
    
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj))
        {
            return true;
        }
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
            
            if (!c1.Equals(c2))
            {
                return false;
            }
        }

        return true;
    }

    public override int GetHashCode()
    {
        // Primes are only really used to spread the values more reasonable and avoid collisions. 
        const int prime1 = 17;
        const int prime2 = 23;
        return GetEqualityComponents()
            .Aggregate(prime1, (current, obj) => current * prime2 + (obj?.GetHashCode() ?? 0));
    }

    protected abstract IEnumerable<object> GetEqualityComponents();
}