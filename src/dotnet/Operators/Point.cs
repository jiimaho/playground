namespace Operators;

public readonly struct Point : IEquatable<Point>
{
    public int X { get; }
    public int Y { get; }
    
    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }

    // Why?
    // Usually best to override this method when overriding Equals
    // Needed for hashed collections for example HashSet<T> and Dictionary<TKey,TValue>
    // HashCodes are NOT guaranteed to be unique, but should be as evenly as possible distributed over a large set of values
    // Default implementation returns the combined hashcodes of the properties
    public override int GetHashCode() => HashCode.Combine(X, Y);

    public bool Equals(Point other) => X == other.X && Y == other.Y;

    public override bool Equals(object? obj) => obj is Point other && Equals(other);
}