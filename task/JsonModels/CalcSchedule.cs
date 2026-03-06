using System.Text.Json.Serialization;

namespace task.JsonModels;

public class CalcSchedule
{
    [JsonPropertyName("derival")]
    public string? Derival { get; set; }

    [JsonPropertyName("arrival")]
    public string? Arrival { get; set; }
}