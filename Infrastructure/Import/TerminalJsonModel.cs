using System.Text.Json.Serialization;

namespace task.Infrastructure.Import;

public sealed class TerminalJsonModel
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("address")]
    public string? Address { get; set; }

    [JsonPropertyName("fullAddress")]
    public string? FullAddress { get; set; }

    [JsonPropertyName("latitude")]
    public string? Latitude { get; set; }

    [JsonPropertyName("longitude")]
    public string? Longitude { get; set; }

    [JsonPropertyName("isPVZ")]
    public bool IsPvz { get; set; }

    [JsonPropertyName("isOffice")]
    public bool IsOffice { get; set; }

    [JsonPropertyName("storage")]
    public bool IsWarehouse { get; set; }

    [JsonPropertyName("calcSchedule")]
    public CalcScheduleJsonModel? CalcSchedule { get; set; }

    [JsonPropertyName("phones")]
    public List<TerminalPhoneJsonModel> Phones { get; set; } = [];
}
