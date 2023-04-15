namespace SystemClock;

public interface ISystemClock
{
    DateTimeOffset UtcNow { get; }
    DateTimeOffset Now => UtcNow.ToLocalTime();
}