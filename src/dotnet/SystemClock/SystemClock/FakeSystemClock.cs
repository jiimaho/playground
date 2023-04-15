namespace SystemClock;

public class FakeSystemClock : ISystemClock
{
    private DateTimeOffset _utcNow = new DateTimeOffset(2000, 1, 1, 0, 00, 00, TimeSpan.Zero).ToUniversalTime();

    public FakeSystemClock()
    {
    }

    public FakeSystemClock(DateTimeOffset now)
    {
        UtcNow = now.ToUniversalTime();
    }

    public FakeSystemClock(ISystemClock clock)
    {
        if (clock == null) throw new ArgumentNullException(nameof(clock));
        UtcNow = clock.UtcNow;
    }

    public DateTimeOffset UtcNow
    {
        get => _utcNow;
        set
        {
            if (_utcNow == value) return;

            _utcNow = value.ToUniversalTime();
        }
    }
    
    public DateTimeOffset Now => UtcNow.ToLocalTime();
    public DateTimeOffset Add(TimeSpan timeSpan) => UtcNow = UtcNow.Add(timeSpan);
    public DateTimeOffset AddYears(int years) => UtcNow = UtcNow.AddYears(years);
    public DateTimeOffset AddMonths(int months) => UtcNow = UtcNow.AddMonths(months);
    public DateTimeOffset AddDays(double days) => UtcNow = UtcNow.AddDays(days);
    public DateTimeOffset AddHours(double hours) => UtcNow = UtcNow.AddHours(hours);
    public DateTimeOffset AddMinutes(double minutes) => UtcNow = UtcNow.AddMinutes(minutes);
    public DateTimeOffset AddSeconds(double seconds) => UtcNow = UtcNow.AddSeconds(seconds);
    public DateTimeOffset AddMilliseconds(double milliseconds) => UtcNow = UtcNow.AddMilliseconds(milliseconds);
    public DateTimeOffset AddTicks(long ticks) => UtcNow = UtcNow.AddTicks(ticks);
}