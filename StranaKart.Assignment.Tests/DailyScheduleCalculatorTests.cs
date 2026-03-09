using System.Globalization;
using StranaKart.Assignment.Domain.Scheduling;

namespace StranaKart.Assignment.Tests;

public class DailyScheduleCalculatorTests
{
    [Fact]
    public void CalculateNextRun_ReturnsSameDayRun_WhenCurrentTimeBeforeRunHour()
    {
        var moscow = FindTimeZone("Europe/Moscow", "Russian Standard Time");
        var referenceTime = ParseOffset("2026-03-07 00:15:00 +03:00");
        const int runHour = 2;

        var result = DailyScheduleCalculator.CalculateNextRun(referenceTime, runHour, moscow);

        Assert.Equal(ParseLocal("2026-03-07 02:00:00"), result.NextRun);
        Assert.Equal(moscow.Id, result.TimeZone);
    }

    [Fact]
    public void CalculateNextRun_ReturnsNextDayRun_WhenCurrentTimeEqualsRunHour()
    {
        var moscow = FindTimeZone("Europe/Moscow", "Russian Standard Time");
        var referenceTime = ParseOffset("2026-03-07 02:00:00 +03:00");
        const int runHour = 2;

        var result = DailyScheduleCalculator.CalculateNextRun(referenceTime, runHour, moscow);

        Assert.Equal(ParseLocal("2026-03-08 02:00:00"), result.NextRun);
        Assert.Equal(moscow.Id, result.TimeZone);
    }

    [Fact]
    public void CalculateNextRun_ReturnsNextDayRun_WhenCurrentTimeAfterRunHour()
    {
        var moscow = FindTimeZone("Europe/Moscow", "Russian Standard Time");
        var referenceTime = ParseOffset("2026-03-07 13:30:00 +03:00");
        const int runHour = 2;

        var result = DailyScheduleCalculator.CalculateNextRun(referenceTime, runHour, moscow);

        Assert.Equal(ParseLocal("2026-03-08 02:00:00"), result.NextRun);
        Assert.Equal(moscow.Id, result.TimeZone);
    }

    [Fact]
    public void CalculateNextRun_ShiftsToNextValidHour_WhenRunHourFallsIntoDstGap()
    {
        var berlin = FindTimeZone("Europe/Berlin", "W. Europe Standard Time");
        var referenceTime = ParseOffset("2026-03-29 00:10:00 +00:00");
        const int runHour = 2;

        var result = DailyScheduleCalculator.CalculateNextRun(referenceTime, runHour, berlin);

        Assert.Equal(ParseLocal("2026-03-29 03:00:00"), result.NextRun);
        Assert.Equal(berlin.Id, result.TimeZone);
    }

    private static DateTimeOffset ParseOffset(string value)
    {
        return DateTimeOffset.ParseExact(
            value,
            "yyyy-MM-dd HH:mm:ss zzz",
            CultureInfo.InvariantCulture);
    }

    private static DateTime ParseLocal(string value)
    {
        return DateTime.ParseExact(
            value,
            "yyyy-MM-dd HH:mm:ss",
            CultureInfo.InvariantCulture,
            DateTimeStyles.None);
    }

    private static TimeZoneInfo FindTimeZone(params string[] ids)
    {
        foreach (var id in ids)
        {
            try
            {
                return TimeZoneInfo.FindSystemTimeZoneById(id);
            }
            catch (TimeZoneNotFoundException)
            {
                // Try next id for cross-platform compatibility.
            }
        }

        throw new TimeZoneNotFoundException(
            $"None of the requested time zones were found: {string.Join(", ", ids)}");
    }
}
