using StranaKart.Assignment.Domain.Scheduling;

namespace StranaKart.Assignment.Infrastructure.Scheduling;

public interface IScheduleService
{
    NextRunInfo GetNextRunInfo();
}