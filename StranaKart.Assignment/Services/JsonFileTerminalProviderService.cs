using System.Text.Json;
using Microsoft.Extensions.Options;
using StranaKart.Assignment.Contracts;
using StranaKart.Assignment.Domain.Entities;
using StranaKart.Assignment.Mapping;
using StranaKart.Assignment.Services.Options;

namespace StranaKart.Assignment.Services;

internal class JsonFileTerminalProviderService : ITerminalProviderService
{
    private readonly IOptions<SyncOptions> _syncOptions;
    private readonly ILogger<JsonFileTerminalProviderService> _logger;
    private readonly IHostEnvironment _hostEnvironment;
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        AllowTrailingCommas = true,
        PropertyNameCaseInsensitive = true
    };

    public JsonFileTerminalProviderService(IOptions<SyncOptions> syncOptions,
        ILogger<JsonFileTerminalProviderService> logger,
        IHostEnvironment hostEnvironment)
    {
        _syncOptions = syncOptions;
        _logger = logger;
        _hostEnvironment = hostEnvironment;
    }

    public async Task<IEnumerable<Office>> GetOfficesAsync(CancellationToken ct)
    {
        var configuredPath = _syncOptions.Value.FilePath;
        var filePath = Path.Combine(_hostEnvironment.ContentRootPath, configuredPath);
        
        _logger.LogDebug("Чтение файла: {FilePath}", filePath);

        CityListDto? response;
        
        // Exception здесь намеренно в лог не пишуться, т.к. их запишет сам воркер
        try
        {
            await using var stream = File.OpenRead(filePath);
            response = await JsonSerializer.DeserializeAsync<CityListDto>(stream, JsonOptions, ct);
        }
        catch (FileNotFoundException)
        {
            _logger.LogWarning("Файл терминалов не найден: {FilePath}.", filePath);
            throw;
        }
        catch (UnauthorizedAccessException)
        {
            _logger.LogWarning("Нет доступа к файлу терминалов: {FilePath}.", filePath);
            throw;
        }
        catch (IOException)
        {
            _logger.LogWarning("Ошибка ввода-вывода при чтении файла терминалов: {FilePath}.", filePath);
            throw;
        }
        catch (JsonException)
        {
            _logger.LogWarning("Файл терминалов повреждён или имеет неверный формат: {FilePath}.", filePath);
            throw;
        }

        if (response?.City is null)
        {
            _logger.LogWarning("Файл пуст или не содержит данных: {FilePath}", filePath);
            return [];
        }

        var result = new List<Office>();
        var skipped = new List<(string? CityCode, string? TerminalId, string Reason)>();

        foreach (var city in response.City)
        {
            if (city.Terminals?.Terminal is null) continue;

            foreach (var terminal in city.Terminals.Terminal)
            {
                var office = TerminalMapper.TryMapToOffice(city, terminal, out var skipReason);

                if (office is not null)
                    result.Add(office);
                else
                    skipped.Add((city.Code, terminal.Id, skipReason!));
            }
        }

        if (_logger.IsEnabled(LogLevel.Debug))
        {
            if (skipped.Count > 0)
                _logger.LogDebug(
                    "Пропущено {Skipped} терминалов: {Details}",
                    skipped.Count,
                    string.Join(", ", skipped.Select(x => $"[CityCode={x.CityCode} TerminalId={x.TerminalId} Reason={x.Reason}]")));

            _logger.LogDebug(
                "Маппинг завершён. Успешно: {Count}, Пропущено: {Skipped}",
                result.Count, skipped.Count);
        }

        return result;
    }
    
    

    
}
