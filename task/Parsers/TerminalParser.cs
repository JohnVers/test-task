using System.Text.Json;
using task.Dto;

namespace task.Parsers;

static class TerminalParser
{
    private static readonly JsonSerializerOptions _options = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public static RootDto ParseJson(string filePath)
    {
        try
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"File not found: {filePath}");

            string json = File.ReadAllText(filePath);

            var root = JsonSerializer.Deserialize<RootDto>(json, _options) 
                ?? throw new JsonException("Failed to deserialize JSON");

            return root;
        }
        catch (Exception ex)
        {
            throw new("Failed terminal parser", ex);
        }
    }
}