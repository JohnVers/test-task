using StranaKart.Assignment.Infrastructure.Scheduling;

namespace StranaKart.Assignment.Domain.Scheduling;

public static class DailyScheduleCalculator
{
    public static NextRunInfo CalculateNextRun(DateTimeOffset referenceTime, int runHour, TimeZoneInfo timeZone)
    {
        var localNow = TimeZoneInfo.ConvertTime(referenceTime, timeZone);
        var nextRunDateTime = CalculateNextRunLocalDateTime(localNow.DateTime, runHour, timeZone);
        return new NextRunInfo(nextRunDateTime, timeZone.Id);
    }

    private static DateTime CalculateNextRunLocalDateTime(DateTime localNow, int runHour, TimeZoneInfo timeZone)
    {
        var nextRun = localNow.Date.AddHours(runHour);

        if (timeZone.IsInvalidTime(nextRun))
            nextRun = nextRun.AddHours(1);

        if (localNow >= nextRun)
        {
            nextRun = localNow.Date.AddDays(1).AddHours(runHour);

            if (timeZone.IsInvalidTime(nextRun))
                nextRun = nextRun.AddHours(1);
        }

        return nextRun;
    }
}
