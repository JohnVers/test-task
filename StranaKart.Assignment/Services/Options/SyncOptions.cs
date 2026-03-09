namespace StranaKart.Assignment.Services.Options;

/// <summary>
/// Настройки синхронизации
/// </summary>
internal sealed class SyncOptions
{
    public const string SectionName = "Sync";

    /// <summary>
    /// Путь к файлу
    /// </summary>
    public required string FilePath { get; set; }
    
}
