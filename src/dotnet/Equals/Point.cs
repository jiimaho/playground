internal struct Point
{
    public int X { get; }
    public int Y { get; }

    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }

    // By default when comparing structs REFLECTION is used to find all properties of the structs.
    // So this override can be beneficial for PERFORMANCE
    public override bool Equals(object? obj)
    {
        return obj is Point other && 
               X == other.X &&
               Y == other.Y;
    }
}