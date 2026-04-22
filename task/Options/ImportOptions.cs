namespace task.Options;

internal sealed class ImportOptions
{
    public required string FilePath { get; init; }

    /// <summary>
    ///     Таймаут импорта в минутах
    /// </summary>
    public int TimeOut { get; init; }

    ///// <summary>
    ///// Запускать импорт при страрте службы
    ///// </summary>
    //public bool DoImportOnStart { get; init; }
}
