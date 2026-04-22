namespace task.Common;

internal sealed record ProperyDescription(string JsonPropertyName, Type? CustomComverterType = null)
{
}
