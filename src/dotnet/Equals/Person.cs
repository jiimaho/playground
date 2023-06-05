internal class Person : IEquatable<Person>
{
    public string Id { get; }
    public string Name { get; }
    public int Age { get; }
    
    // If we don't override this method then comparisons for Person will always return false
    // Worth to override even if we skip IEquatable<Person>
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Person)obj);
    }

    // This method is used by underlying comparisons methods and returns a unique value representing the object
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    // Required by IEquatable<Person>. Makes comparisons for Person much faster since we skip boxing
    public bool Equals(Person? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Id == other.Id;
    }
}