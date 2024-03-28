using System.Diagnostics;

namespace Disasters.GraphQL;

public static class Tracing
{
    public static ActivitySource DisastersGraphQl { get; } = new ActivitySource("Disasters.GraphQL");
}