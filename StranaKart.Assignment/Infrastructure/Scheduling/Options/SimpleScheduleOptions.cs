namespace StranaKart.Assignment.Infrastructure.Scheduling.Options;

/// <summary>
/// Настройки расписания
/// </summary>
public sealed class SimpleScheduleOptions
{
    public const string SectionName = "SimpleSchedule";

    /// <summary>
    /// Таймзона 
    /// </summary>
    public required string TimeZone { get; set; }
    
    /// <summary>
    /// Час выполнения
    /// </summary>
    public required int Hour { get; set; }
}
