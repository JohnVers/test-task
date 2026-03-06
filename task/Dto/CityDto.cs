using System.Text.Json.Serialization;

namespace task.Dto;

public class CityDto
{
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("code")]
    public string? Code { get; set; }

    [JsonPropertyName("cityID")]
    public int? CityId { get; set; }

    [JsonPropertyName("terminals")]
    public TerminalsWrapperDto? Terminals { get; set; }
}
