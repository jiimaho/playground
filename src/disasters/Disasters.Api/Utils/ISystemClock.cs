namespace Disasters.Api.Utils;

public interface ISystemClock
{
    DateTimeOffset UtcNow { get; }
    DateTimeOffset Now => UtcNow.ToLocalTime();
}