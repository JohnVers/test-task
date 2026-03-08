namespace task.Options;

public class FileSettings
{
    public const string Section = "FileSettings";
    
    /// <summary>
    /// Путь к файлу
    /// </summary>
    public string FilePath { get; set; } = string.Empty;
}