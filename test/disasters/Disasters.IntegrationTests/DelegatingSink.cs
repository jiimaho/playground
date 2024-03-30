using Serilog.Core;
using Serilog.Events;

namespace Disasters.IntegrationTests;

public class DelegatingSink(Action<LogEvent> action) : ILogEventSink
{
    public void Emit(LogEvent logEvent) => action.Invoke(logEvent);
}