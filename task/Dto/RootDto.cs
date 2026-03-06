using System.Text.Json.Serialization;

namespace task.Dto;

class RootDto
{
    [JsonPropertyName("city")]
    public CityDto[] City { get; set; } = [];
}