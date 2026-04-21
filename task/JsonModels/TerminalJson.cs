using System.Text.Json;
using System.Text.Json.Serialization;

namespace task.JsonModels;

// Терминал (объект внутри массива terminal)
public class TerminalJson
{
	[JsonPropertyName("id")]
	public string Id { get; set; } = string.Empty;

	[JsonPropertyName("name")]
	public string Name { get; set; } = string.Empty;

	[JsonPropertyName("address")]
	public string Address { get; set; } = string.Empty;

	[JsonPropertyName("fullAddress")]
	public string? FullAddress { get; set; }

	[JsonPropertyName("isPVZ")]
	public bool IsPVZ { get; set; }

	[JsonPropertyName("isOffice")]
	public bool IsOffice { get; set; }

	[JsonPropertyName("storage")]
	public bool Storage { get; set; }

	[JsonPropertyName("mainPhone")]
	public string? MainPhone { get; set; }

	[JsonPropertyName("phones")]
	public List<PhoneJson>? Phones { get; set; }

	[JsonPropertyName("latitude")]
	public string? Latitude { get; set; }

	[JsonPropertyName("longitude")]
	public string? Longitude { get; set; }

	[JsonPropertyName("worktables")]
	public JsonElement Worktables { get; set; }
}