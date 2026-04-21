using System.Text.Json.Serialization;

namespace task.Infrastructure.Import;

public sealed class TerminalDictionaryRoot
{
    [JsonPropertyName("city")]
    public List<CityJsonModel> Cities { get; set; } = [];
}
