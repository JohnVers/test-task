using System.Text.Json.Serialization;

namespace task.JsonModels;

// Контейнер с массивом терминалов
public class TerminalsContainerJson
{
	[JsonPropertyName("terminal")]
	public List<TerminalJson> Terminal { get; set; } = new();
}
