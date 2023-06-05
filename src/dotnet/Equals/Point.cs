// ReSharper disable MemberCanBePrivate.Global
namespace Equals;

internal readonly struct Point : IEquatable<Point>
{
    public int X { get; }
    public int Y { get; }

    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }
    
    // Why?
    // Gives greater performance since we don't need to do boxing
    public bool Equals(Point other) => X == other.X && Y == other.Y;
    
    // Why?
    // By default when comparing structs REFLECTION is used to find all properties of the structs.
    // So this override can be beneficial for PERFORMANCE
    public override bool Equals(object? obj) => obj is Point other && Equals(other);

    // Why?
    // Usually best to override this method when overriding Equals
    // Needed for hashed collections for example HashSet<T> and Dictionary<TKey,TValue>
    public override int GetHashCode() => HashCode.Combine(X, Y);
}