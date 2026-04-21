using System.Text.Json.Serialization;

namespace task.Infrastructure.Import;

public sealed class CalcScheduleJsonModel
{
    [JsonPropertyName("arrival")]
    public string? Arrival { get; set; }

    [JsonPropertyName("derival")]
    public string? Derival { get; set; }
}
