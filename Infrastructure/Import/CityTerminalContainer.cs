using System.Text.Json.Serialization;

namespace task.Infrastructure.Import;

public sealed class CityTerminalContainer
{
    [JsonPropertyName("terminal")]
    public List<TerminalJsonModel> Terminals { get; set; } = [];
}
