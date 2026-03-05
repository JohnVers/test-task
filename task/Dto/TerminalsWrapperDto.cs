using System.Text.Json.Serialization;

namespace task.Dto;

public class TerminalsWrapperDto
{
    [JsonPropertyName("terminal")]
    public List<TerminalDto> Terminal { get; set; } = [];
}
