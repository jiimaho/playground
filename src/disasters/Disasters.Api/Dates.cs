namespace Disasters.Api;

public static class Dates
{
    public static DateTimeOffset BeginningOfDay(this DateOnly date)
    {
        return new DateTimeOffset(date.Year, date.Month, date.Day, 0, 0, 0, TimeSpan.Zero);
    }
}