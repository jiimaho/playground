using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Disasters.Api;

public static class Trace
{
    public static class DisastersApi
    {
        private static ActivitySource Source { get; }= new("Disasters.Api");
        
        public static Activity? StartActivity(
            Type type, 
            ActivityKind kind = ActivityKind.Internal, 
            [CallerMemberName] string name = "")
        {
            return Source.StartActivity($"{type.Name}.{name}", kind);
        }
    }
}