namespace Disasters.Api.Clock;

public class SystemClock : ISystemClock
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
    public DateTimeOffset Now => UtcNow.ToLocalTime();
}