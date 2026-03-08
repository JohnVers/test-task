using System.Text.Json.Serialization;

namespace Application.JsonModels;

public class Root
{
    [JsonPropertyName("city")]
    public List<City> Cities { get; set; } = new();
}