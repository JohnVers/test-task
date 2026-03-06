using System.Text.Json.Serialization;

namespace task.Dto;

public class CalcScheduleDto
{
    [JsonPropertyName("derival")]
    public string? Derival { get; set; }

    [JsonPropertyName("arrival")]
    public string? Arrival { get; set; }
}
