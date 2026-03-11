namespace task.Options;

internal sealed class ImportOptions
{
    public required string FilePath { get; init; }

    /// <summary>
    ///     Timeout in minutes
    /// </summary>
    public int TimeOut { get; init; }
}
