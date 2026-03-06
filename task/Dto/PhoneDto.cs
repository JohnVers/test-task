using System.Text.Json.Serialization;

namespace task.Dto;

public class PhoneDto
{
    [JsonPropertyName("number")]
    public string? Number { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("comment")]
    public string? Comment { get; set; }
}
