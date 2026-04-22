using Application;
using Domain.Entities;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Infrastructure
{
    public class TerminalImportBackgroundService : BackgroundService
    {
        private readonly ILogger<TerminalImportBackgroundService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly TimeZoneInfo _mskZone;

        public TerminalImportBackgroundService(
            ILogger<TerminalImportBackgroundService> logger,
            IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;

            _mskZone = TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time");
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("TerminalImportBackgroundService started");

            // Выполняем импорт при старте
            await ImportTerminalsAsync(cancellationToken);

            while (!cancellationToken.IsCancellationRequested)
            {
                var now = TimeZoneInfo.ConvertTime(DateTime.UtcNow, _mskZone);
                var nextRun = GetNextRunTime(now);
                var delay = nextRun - now;

                _logger.LogInformation("Next import scheduled at {NextRun:yyyy-MM-dd HH:mm:ss} MSK (in {DelayMinutes} minutes)",
                    nextRun, delay.TotalMinutes);

                await Task.Delay(delay, cancellationToken);

                if (cancellationToken.IsCancellationRequested)
                    break;

                await ImportTerminalsAsync(cancellationToken);
            }

            _logger.LogInformation("TerminalImportBackgroundService stopped");
        }

        private DateTime GetNextRunTime(DateTime now)
        {
            var next = new DateTime(now.Year, now.Month, now.Day, 2, 0, 0);
            if (now >= next)
                next = next.AddDays(1);
            return next;
        }

        private async Task ImportTerminalsAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting terminal import");
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            try
            {
                var fullPath = Path.Combine(AppContext.BaseDirectory, "files/terminals.json");
                if (!File.Exists(fullPath))
                {
                    _logger.LogError("Terminal file not found at {FilePath}", fullPath);
                    return;
                }

                var json = await File.ReadAllTextAsync(fullPath, cancellationToken);

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString
                };

                var root = JsonSerializer.Deserialize<RootJson>(json, options);

                if (root?.City == null)
                {
                    _logger.LogWarning("No cities found in JSON");
                    return;
                }

                var offices = new List<Office>();

                foreach (var city in root.City)
                {
                    if (city.Terminals?.Terminal == null)
                        continue;

                    foreach (var terminal in city.Terminals.Terminal)
                    {
                        var office = new Office
                        {
                            Code = terminal.Code,
                            CityCode = terminal.CityCode > 0 ? terminal.CityCode : city.CityId ?? 0,
                            Uuid = terminal.Uuid,
                            Type = ParseOfficeType(terminal.Type),
                            CountryCode = terminal.CountryCode ?? string.Empty,
                            Latitude = ParseDouble(terminal.Latitude),
                            Longitude = ParseDouble(terminal.Longitude),
                            AddressRegion = terminal.AddressRegion ?? city.Name,
                            AddressCity = terminal.AddressCity ?? city.Name,
                            AddressStreet = terminal.AddressStreet,
                            AddressHouseNumber = terminal.AddressHouseNumber,
                            AddressApartment = terminal.AddressApartment,
                            WorkTime = terminal.WorkTime ?? string.Empty,
                            Phones = new List<Phone>()
                        };

                        foreach (var phoneDto in terminal.Phones)
                        {
                            if (!string.IsNullOrEmpty(phoneDto.Number))
                            {
                                office.Phones.Add(new Phone
                                {
                                    PhoneNumber = phoneDto.Number,
                                    Additional = phoneDto.Additional
                                });
                            }
                        }

                        offices.Add(office);
                    }
                }

                _logger.LogInformation("Loaded {Count} terminals from JSON", offices.Count);

                using var scope = _scopeFactory.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<DellinDictionaryDbContext>();

                await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

                var oldOfficesCount = await dbContext.Offices.CountAsync(cancellationToken);
                _logger.LogInformation("Deleting {OldCount} existing records", oldOfficesCount);

                await dbContext.Phones.ExecuteDeleteAsync(cancellationToken);
                await dbContext.Offices.ExecuteDeleteAsync(cancellationToken);

                await dbContext.Offices.AddRangeAsync(offices, cancellationToken);

                await dbContext.SaveChangesAsync(cancellationToken);

                await transaction.CommitAsync(cancellationToken);

                _logger.LogInformation("Saved {NewCount} terminals with phones", offices.Count);

                stopwatch.Stop();
                _logger.LogInformation("Import completed in {ElapsedMilliseconds} ms", stopwatch.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error importing terminals");
            }
        }

        private static OfficeType? ParseOfficeType(string? type)
        {
            return type?.ToUpper() switch
            {
                "PVZ" => OfficeType.PVZ,
                "POSTAMAT" => OfficeType.POSTAMAT,
                "WAREHOUSE" => OfficeType.WAREHOUSE,
                _ => null
            };
        }

        private static double ParseDouble(string? value)
        {
            if (string.IsNullOrEmpty(value))
                return 0;

            return double.TryParse(value, System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture, out var result) ? result : 0;
        }
    }
}
