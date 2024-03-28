using System.Diagnostics;

namespace Disasters.Api;

public static class Tracing
{
    public static ActivitySource DisastersApi { get; }= new("Disasters.Api");
}