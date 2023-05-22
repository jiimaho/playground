namespace SystemClock;

public static class TimeUtils
{
    public static DateTimeOffset BeginningOfDay(this DateTimeOffset dateTimeOffset) 
        => new DateTimeOffset(dateTimeOffset.Year, dateTimeOffset.Month, dateTimeOffset.Day, 0, 0, 0, dateTimeOffset.Offset);
}