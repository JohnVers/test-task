using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using task.Data;
using task.Dto;
using task.Models;

namespace task.Services;

public class TerminalsImportService
{
    private readonly DellinDictionaryDbContext _context;
    private readonly ILogger<TerminalsImportService> _logger;
    private readonly IConfiguration _configuration;
    private const int BatchSize = 500;

    public TerminalsImportService(
        DellinDictionaryDbContext context,
        ILogger<TerminalsImportService> logger,
        IConfiguration configuration)
    {
        _context = context;
        _logger = logger;
        _configuration = configuration;
    }

    public async Task ImportAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Начало импорта терминалов");

            var offices = await LoadFromJsonAsync(cancellationToken);
            _logger.LogInformation("Загружено {Count} терминалов из JSON", offices.Count);

            await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

            var oldCount = await _context.Offices.CountAsync(cancellationToken);
            await _context.Phones.ExecuteDeleteAsync(cancellationToken);
            await _context.Offices.ExecuteDeleteAsync(cancellationToken);
            _logger.LogInformation("Удалено {OldCount} старых записей", oldCount);

            int savedCount = 0;
            for (int i = 0; i < offices.Count; i += BatchSize)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var batch = offices.GetRange(i, Math.Min(BatchSize, offices.Count - i));
                await _context.Offices.AddRangeAsync(batch, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                savedCount += batch.Count;
                _context.ChangeTracker.Clear();
            }

            await transaction.CommitAsync(cancellationToken);
            _logger.LogInformation("Сохранено {NewCount} новых терминалов", savedCount);
        }
        catch (Exception ex)
        {
            _logger.LogError("Ошибка импорта: {Exception}", ex);
            throw;
        }
    }

    private async Task<List<Office>> LoadFromJsonAsync(CancellationToken cancellationToken)
    {
        var relativePath = _configuration.GetValue<string>("TerminalsFilePath") ?? "files/terminals.json";
        var filePath = Path.Combine(AppContext.BaseDirectory, relativePath);

        if (!File.Exists(filePath))
            filePath = Path.Combine(Directory.GetCurrentDirectory(), relativePath);

        if (!File.Exists(filePath))
            throw new FileNotFoundException($"Файл терминалов не найден: {filePath}");

        await using var stream = File.OpenRead(filePath);

        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var root = await JsonSerializer.DeserializeAsync<TerminalsRoot>(stream, options, cancellationToken)
            ?? throw new InvalidOperationException("Не удалось десериализовать JSON файл");

        var offices = new List<Office>();
        foreach (var city in root.City)
        {
            if (city.Terminals?.Terminal is null) 
                continue;
            foreach (var terminal in city.Terminals.Terminal)
                offices.Add(MapToOffice(city, terminal));
        }

        return offices;
    }

    private static Office MapToOffice(CityDto city, TerminalDto terminal)
    {
        var lat = double.TryParse(terminal.Latitude,
            System.Globalization.NumberStyles.Any,
            System.Globalization.CultureInfo.InvariantCulture, out var latVal) ? latVal : 0.0;
        var lon = double.TryParse(terminal.Longitude,
            System.Globalization.NumberStyles.Any,
            System.Globalization.CultureInfo.InvariantCulture, out var lonVal) ? lonVal : 0.0;

        var office = new Office
        {
            Code = terminal.Id,
            CityCode = city.CityId ?? 0,
            Uuid = city.Code,
            Type = terminal.IsPvz ? OfficeType.PVZ : OfficeType.WAREHOUSE,
            CountryCode = "RU",
            Coordinates = new Coordinates { Latitude = lat, Longitude = lon },
            AddressCity = city.Name,
            AddressStreet = terminal.Address,
            WorkTime = terminal.CalcSchedule?.Derival ?? string.Empty,
        };

        var firstPhone = terminal.Phones?.FirstOrDefault();
        if (firstPhone?.Number is not null)
        {
            office.Phones = new Phone
            {
                PhoneNumber = firstPhone.Number,
                Additional = string.IsNullOrEmpty(firstPhone.Comment) ? null : firstPhone.Comment,
            };
        }

        return office;
    }
}
