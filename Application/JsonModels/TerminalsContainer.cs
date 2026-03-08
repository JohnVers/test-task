using System.Text.Json.Serialization;

namespace Application.JsonModels;

public class TerminalsContainer
{
    [JsonPropertyName("terminal")]
    public List<Terminal> Terminals { get; set; } = new();
}