using System.Text.Json.Serialization;

namespace task.Dto;

public class TerminalDto
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

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

    [JsonPropertyName("phones")]
    public List<PhoneDto> Phones { get; set; } = [];

    [JsonPropertyName("calcSchedule")]
    public CalcScheduleDto? CalcSchedule { get; set; }
}
