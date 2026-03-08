namespace task.Options;

/// <summary>
/// Настройки джобы
/// </summary>
public class JobSettings
{
    public const string Section = "JobSettings";

    /// <summary>
    /// Расписание джобы по импорту терминалов
    /// </summary>
    public string ImportTerminalsSchedule { get; set; } = string.Empty;
}