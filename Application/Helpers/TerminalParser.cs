using System.Text.Json;
using Application.JsonModels;

namespace Application.Helpers;

internal static class TerminalParser
{
    private static readonly JsonSerializerOptions _options = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };
    
    public static Root Parse(string filePath)
    {
        try
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("Файл {FilePath} не найден", filePath);

            var jsonFile = File.ReadAllText(filePath);

            var root = JsonSerializer.Deserialize<Root>(jsonFile, _options) ??
                       throw new JsonException("Ошибка десериализации файла");
            
            return root;
        }
        catch (Exception e)
        {
            throw new Exception("Ошибка парсинга файла", e);
        }
    }
}