using System.Text.Json.Serialization;

namespace task.JsonModels;

public class TerminalsContainer
{
    [JsonPropertyName("terminal")]
    public List<Terminal> Terminals { get; set; } = new();
}