using System.Text.Json.Serialization;

namespace task.Infrastructure.Import;

public sealed class CityJsonModel
{
    [JsonPropertyName("cityID")]
    public int? CityId { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("code")]
    public string? RegionCode { get; set; }

    [JsonPropertyName("terminals")]
    public CityTerminalContainer? TerminalsContainer { get; set; }
}
