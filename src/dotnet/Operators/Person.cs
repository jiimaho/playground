// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable RedundantToStringCallForValueType
namespace Operators;

public class Person
{
    public string Id { get; }
    public string Name { get; }

    public Person(string id, string name)
    {
        Id = id;
        Name = name;
    }
    
    public override string ToString() => $"Id: {Id}, Name: {Name}";

    // Why?
    // Usually best to override this method when overriding Equals
    // Needed for hashed collections for example HashSet<T> and Dictionary<TKey,TValue>
    // HashCodes are NOT guaranteed to be unique, but should be as evenly as possible distributed over a large set of values
    // Default implementation returns the memory address of the object
    public override int GetHashCode() => Id.GetHashCode();

    // Why?
    // You can customize the equality operator to do whatever you want.
    public static bool operator ==(Person p, Uri uri) => false;
    
    // Why?
    // You can customize the inequality operator to do whatever you want.
    public static bool operator !=(Person p, Uri uri) => false;
    
    // Why?
    // You can customize the plus operator to do whatever you want.
    public static Person operator +(Person p, Point o) => new(p.Id, $"I am indeed a very strange person" +
                                                                    $" seeing as {p.Name} is my name and " +
                                                                    $"I also have a point {o.X} {o.Y}");
    
    // Why?
    // Allows to very easily convert from one type to another. Can be very useful.
    public static implicit operator Person(Point point)
        => new(point.X.ToString() + point.Y.ToString(), $"Your name as a point is {point.X} {point.Y}");
    
    // Why?
    // Allows to very easily convert from one type to another. Can be very useful.
    public static explicit operator Person(HttpClient point)
        => new(point.BaseAddress!.ToString(), $"Your name as a httpclient is {point.Timeout}");
}