using Microsoft.Extensions.Options;
using StranaKart.Assignment.Domain.Scheduling;
using StranaKart.Assignment.Infrastructure.Scheduling.Options;

namespace StranaKart.Assignment.Infrastructure.Scheduling;

internal class SimpleScheduleService : IScheduleService
{
    private readonly IOptions<SimpleScheduleOptions> _scheduleOptions;
    private readonly TimeProvider _timeProvider;

    public SimpleScheduleService(IOptions<SimpleScheduleOptions> scheduleOptions, TimeProvider timeProvider)
    {
        _scheduleOptions = scheduleOptions;
        _timeProvider = timeProvider;
    }

    public NextRunInfo GetNextRunInfo()
    {
        var scheduleTimeZone = _scheduleOptions.Value.TimeZone;
        var scheduleHour = _scheduleOptions.Value.Hour;

        var tz = TimeZoneInfo.FindSystemTimeZoneById(scheduleTimeZone);
        var referenceTime = _timeProvider.GetUtcNow();

        return DailyScheduleCalculator.CalculateNextRun(referenceTime, scheduleHour, tz);
    }
}
