using System.Security.Cryptography;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using task.Data;
using task.Dto;
using task.Entities;

namespace task.Services;

public class TerminalsImportService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IHostEnvironment _environment;
    private readonly ILogger<TerminalsImportService> _logger;
    private readonly IConfiguration _configuration;

    private string? _lastLoadedFileHash;

    private static readonly TimeZoneInfo MskTimeZone = GetMskTimeZone();

    private static TimeZoneInfo GetMskTimeZone()
    {
        try
        {
            return TimeZoneInfo.FindSystemTimeZoneById("Europe/Moscow");
        }
        catch
        {
            return TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time");
        }
    }
    private static readonly TimeSpan ImportTime = new(2, 0, 0); // 02:00 MSK

    public TerminalsImportService(
        IServiceProvider serviceProvider,
        IHostEnvironment environment,
        ILogger<TerminalsImportService> logger,
        IConfiguration configuration)
    {
        _serviceProvider = serviceProvider;
        _environment = environment;
        _logger = logger;
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Служба импорта терминалов запущена. Следующий импорт в 02:00 MSK");

        var runOnStartup = _configuration.GetValue<bool>("TerminalsImport:RunOnStartup");
        if (runOnStartup)
        {
            try
            {
                _logger.LogInformation("Запуск импорта при старте (RunOnStartup=true)");
                await RunImportAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка импорта при старте: {Exception}", ex.Message);
            }
        }

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var delay = GetDelayUntilNextImport();
                _logger.LogInformation("Ожидание до следующего импорта: {Delay}", delay);

                await Task.Delay(delay, stoppingToken);

                if (stoppingToken.IsCancellationRequested)
                    break;

                await RunImportAsync(stoppingToken);
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка импорта: {Exception}", ex.Message);
            }
        }

        _logger.LogInformation("Служба импорта терминалов остановлена");
    }

    private static async Task<string> ComputeFileHashAsync(string filePath, CancellationToken cancellationToken)
    {
        await using var stream = File.OpenRead(filePath);
        var hash = await SHA256.HashDataAsync(stream, cancellationToken);
        return Convert.ToHexString(hash);
    }

    private static TimeSpan GetDelayUntilNextImport()
    {
        var nowMsk = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, MskTimeZone);
        var todayImport = nowMsk.Date.Add(ImportTime);

        var nextImport = nowMsk < todayImport
            ? todayImport
            : todayImport.AddDays(1);

        var nextImportUtc = TimeZoneInfo.ConvertTimeToUtc(nextImport, MskTimeZone);
        return nextImportUtc - DateTime.UtcNow;
    }

    private async Task RunImportAsync(CancellationToken stoppingToken)
    {
        var sw = System.Diagnostics.Stopwatch.StartNew();

        try
        {
            var jsonPath = Path.Combine(_environment.ContentRootPath, "files", "terminals.json");

            if (!File.Exists(jsonPath))
            {
                _logger.LogError("Файл не найден: {Path}", jsonPath);
                return;
            }

            var currentHash = await ComputeFileHashAsync(jsonPath, stoppingToken);
            if (_lastLoadedFileHash is not null && _lastLoadedFileHash == currentHash)
            {
                _logger.LogInformation("Файл не изменился (хэш совпадает), загрузка пропущена");
                return;
            }

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            await using var stream = File.OpenRead(jsonPath);
            var root = await JsonSerializer.DeserializeAsync<TerminalsRootDto>(stream, options, stoppingToken);

            if (root?.City == null)
            {
                _logger.LogWarning("JSON не содержит данных о городах");
                return;
            }

            var offices = MapToOffices(root);
            _logger.LogInformation("Загружено {Count} терминалов из JSON", offices.Count);

            await using var scope = _serviceProvider.CreateAsyncScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<DellinDictionaryDbContext>();

            await dbContext.Database.EnsureCreatedAsync(stoppingToken);

            var oldCount = await dbContext.Offices.CountAsync(stoppingToken);
            dbContext.Offices.RemoveRange(dbContext.Offices);
            await dbContext.SaveChangesAsync(stoppingToken);
            _logger.LogInformation("Удалено {OldCount} старых записей", oldCount);

            if (offices.Count > 0)
            {
                const int batchSize = 5000;
                for (var i = 0; i < offices.Count; i += batchSize)
                {
                    var batch = offices.Skip(i).Take(batchSize).ToList();
                    await dbContext.Offices.AddRangeAsync(batch, stoppingToken);
                    await dbContext.SaveChangesAsync(stoppingToken);
                }
                _logger.LogInformation("Сохранено {NewCount} новых терминалов", offices.Count);
            }

            _lastLoadedFileHash = currentHash;
            sw.Stop();
            _logger.LogInformation("Импорт завершён за {ElapsedMs} мс", sw.ElapsedMilliseconds);
        }
        catch (Exception ex)
        {
            sw.Stop();
            _logger.LogError(ex, "Ошибка импорта: {Exception}", ex.Message);
        }
    }

    private static List<Office> MapToOffices(TerminalsRootDto root)
    {
        var offices = new List<Office>();

        foreach (var city in root.City)
        {
            if (city.CityId is not { } cityCode)
                continue;

            var terminals = city.Terminals?.Terminal ?? [];

            foreach (var terminal in terminals)
            {
                if (string.IsNullOrEmpty(terminal.Id))
                    continue;

                if (!int.TryParse(terminal.Id, out var officeId))
                    continue;

                var lat = double.TryParse(terminal.Latitude, System.Globalization.CultureInfo.InvariantCulture, out var latitude)
                    ? latitude
                    : 0;
                var lon = double.TryParse(terminal.Longitude, System.Globalization.CultureInfo.InvariantCulture, out var longitude)
                    ? longitude
                    : 0;

                var workTime = terminal.CalcSchedule?.Derival
                    ?? terminal.CalcSchedule?.Arrival
                    ?? string.Empty;

                var type = terminal.IsPvz
                    ? OfficeType.PVZ
                    : terminal.IsOffice
                        ? OfficeType.WAREHOUSE
                        : OfficeType.WAREHOUSE;

                var office = new Office
                {
                    Id = officeId,
                    Code = terminal.Id,
                    CityCode = cityCode,
                    CountryCode = "RU",
                    Coordinates = new Coordinates { Latitude = lat, Longitude = lon },
                    AddressCity = city.Name,
                    AddressStreet = terminal.FullAddress ?? terminal.Address,
                    WorkTime = workTime,
                    Type = type
                };

                foreach (var phoneDto in terminal.Phones)
                {
                    if (!string.IsNullOrEmpty(phoneDto.Number))
                    {
                        office.Phones.Add(new Phone
                        {
                            PhoneNumber = phoneDto.Number,
                            Additional = phoneDto.Type
                        });
                    }
                }

                offices.Add(office);
            }
        }

        return offices;
    }
}
