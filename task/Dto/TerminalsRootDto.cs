using System.Text.Json.Serialization;

namespace task.Dto;

public class TerminalsRootDto
{
    [JsonPropertyName("city")]
    public List<CityDto> City { get; set; } = [];
}
