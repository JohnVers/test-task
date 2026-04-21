namespace task.Infrastructure.Utilities;

/// <summary>
/// Расчёт интервалов до следующего запуска по московскому времени.
/// </summary>
public static class MoscowScheduling
{
    public static TimeSpan GetDelayUntilNextRun(DateTimeOffset utcNow, TimeSpan dailyLocalTime)
    {
        var moscowTimeZone = ResolveMoscowTimeZone();
        var moscowNow = TimeZoneInfo.ConvertTime(utcNow, moscowTimeZone);
        var nextRun = new DateTimeOffset(
            moscowNow.Year,
            moscowNow.Month,
            moscowNow.Day,
            dailyLocalTime.Hours,
            dailyLocalTime.Minutes,
            dailyLocalTime.Seconds,
            moscowTimeZone.GetUtcOffset(moscowNow));

        if (moscowNow >= nextRun)
        {
            nextRun = nextRun.AddDays(1);
        }

        return nextRun - moscowNow;
    }

    /// <summary>
    /// Запуск в 02:00 по Москве.
    /// </summary>
    public static TimeSpan GetDelayUntilNextDailyRunAtTwoAmMsk(DateTimeOffset utcNow) => GetDelayUntilNextRun(utcNow, TimeSpan.FromHours(2));

    public static TimeZoneInfo ResolveMoscowTimeZone()
    {
        try
        {
            return TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time");
        }
        catch (TimeZoneNotFoundException)
        {
            return TimeZoneInfo.FindSystemTimeZoneById("Europe/Moscow");
        }
    }
}
