using System.Text.Json.Serialization;

namespace Application.JsonModels;

public class WorktableEntry
{
    [JsonPropertyName("department")]
    public string? Department { get; set; }

    [JsonPropertyName("monday")]
    public string? Monday { get; set; }

    [JsonPropertyName("tuesday")]
    public string? Tuesday { get; set; }

    [JsonPropertyName("wednesday")]
    public string? Wednesday { get; set; }

    [JsonPropertyName("thursday")]
    public string? Thursday { get; set; }

    [JsonPropertyName("friday")]
    public string? Friday { get; set; }

    [JsonPropertyName("saturday")]
    public string? Saturday { get; set; }

    [JsonPropertyName("sunday")]
    public string? Sunday { get; set; }

    [JsonPropertyName("timetable")]
    public string? Timetable { get; set; }
}