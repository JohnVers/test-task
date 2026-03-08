using System.Text.Json.Serialization;

namespace Application.JsonModels;

public class Phone
{
    [JsonPropertyName("number")]
    public string? Number { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("comment")]
    public string? Comment { get; set; }

    [JsonPropertyName("primary")]
    public bool Primary { get; set; }
}