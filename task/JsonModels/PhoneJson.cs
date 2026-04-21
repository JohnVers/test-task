using System.Text.Json.Serialization;

namespace task.JsonModels;

// Телефон в JSON
public class PhoneJson
{
	[JsonPropertyName("number")]
	public string Number { get; set; } = string.Empty;

	[JsonPropertyName("comment")]
	public string? Comment { get; set; }
}
