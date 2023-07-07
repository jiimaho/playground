namespace Disasters.Api.Clock;

public interface ISystemClock
{
    DateTimeOffset UtcNow { get; }
    DateTimeOffset Now => UtcNow.ToLocalTime();
}