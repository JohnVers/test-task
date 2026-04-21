using System.Text.Json.Serialization;

namespace task.JsonModels;

// Город
public class CityJson
{
	[JsonPropertyName("name")]
	public string Name { get; set; } = string.Empty;

	[JsonPropertyName("cityID")]
	public int? CityID { get; set; }

	[JsonPropertyName("code")]
	public string? Code { get; set; }

	[JsonPropertyName("terminals")]
	public TerminalsContainerJson Terminals { get; set; } = new();
}
