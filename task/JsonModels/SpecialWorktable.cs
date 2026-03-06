using System.Text.Json.Serialization;

namespace task.JsonModels;

public class SpecialWorktable
{
    [JsonPropertyName("receive")]
    public List<string>? Receive { get; set; }

    [JsonPropertyName("giveout")]
    public List<string>? Giveout { get; set; }
}