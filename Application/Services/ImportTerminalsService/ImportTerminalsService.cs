using System.Text.Json;
using Application.JsonModels;
using Application.Services.ImportTerminalsService.Interfaces;
using Microsoft.Extensions.Logging;
using Persistence.Common;

namespace Application.Services.ImportTerminalsService;

/// <inheritdoc/>
public class ImportTerminalsService : IImportTerminalsService
{
    private readonly ILogger<ImportTerminalsService> _logger;

    public ImportTerminalsService(DellinDictionaryDbContext dbContext,
        ILogger<ImportTerminalsService> logger)
    {
        _logger = logger;
    }

    public async Task ImportAsync(string filePath, CancellationToken cancellationToken)
    {
        try
        {
            if (!File.Exists(filePath))
            {
                _logger.LogError("Файл {FilePath} не найден", filePath);
                return;
            }

            var jsonFile = await File.ReadAllTextAsync(filePath, cancellationToken);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            
            var root = JsonSerializer.Deserialize<Root>(jsonFile, options);
            
            if (root == null)
            {
                _logger.LogWarning("Не удалось десериализовать JSON файл");
                return;
            }

            _logger.LogInformation("Загружено {Count} терминалов из JSON", root.Cities.Count);

            
            await ImportToDatabaseAsync(root.Cities, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка импорта терминалов");
        }
    }
    
    private async Task ImportToDatabaseAsync(IEnumerable<City> cities, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}