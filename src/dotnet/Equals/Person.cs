// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace Equals;

internal class Person : IEquatable<Person>
{
    public string Id { get; }

    public string Name { get; }

    public int Age { get; }

    public Person(string id, string name, int age)
    {
        Id = id;
        Name = name;
        Age = age;
    }
    
    public static Person Empty => new(string.Empty, string.Empty, 0);

    // Why?
    // Must override to support value-based equality
    public override bool Equals(object? obj) => obj is Person other && Equals(other);

    // Why?
    // Makes comparisons for Person much more PERFORMANT since we skip boxing. Required by IEquatable<Person>. 
    // If we don't implement this then the default implementation will be used which will use the Equals method taking an object
    public bool Equals(Person? other) => other != null && Id == other.Id;
    
    // Why?
    // Usually best to override this method when overriding Equals
    // Do note that a hashcode is NOT a unique identifier for the object, and collisions can occur
    // Still this hashcode is used for example in hashed collections like HashSet<T> and Dictionary<TKey,TValue>
    public override int GetHashCode() => Id.GetHashCode();
}