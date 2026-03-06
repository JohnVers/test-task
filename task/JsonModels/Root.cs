using System.Text.Json.Serialization;

namespace task.JsonModels;

public class Root
{
    [JsonPropertyName("city")]
    public List<City> Cities { get; set; } = new();
}