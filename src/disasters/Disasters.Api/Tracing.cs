using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Disasters.Api;

public static class Tracing
{
    public static ActivitySource DisastersApi { get; }= new("Disasters.Api");
    
    public static Activity? StartCustomActivity(Type type, [CallerMemberName] string name = "")
    {
        return DisastersApi.StartActivity($"{type.Name}.{name}");
    }
}