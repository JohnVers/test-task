using System.Text.Json.Serialization;

namespace task.Infrastructure.Import;

public sealed class TerminalPhoneJsonModel
{
    [JsonPropertyName("number")]
    public string? Number { get; set; }

    [JsonPropertyName("comment")]
    public string? Comment { get; set; }
}
