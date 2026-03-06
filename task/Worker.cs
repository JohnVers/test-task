using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Runtime.InteropServices;
using task.Data;
using task.Data.Entities;
using task.Dto;
using task.Parsers;

namespace task;

class Worker(IDbContextFactory<DellinDictionaryDbContext> contextFactory, 
             IOptions<WorkerOptions> options, 
             IMapper mapper,
             ILogger<Worker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var mskTz = GetMoscowTimeZone();

        while (!stoppingToken.IsCancellationRequested)
        {
            var delay = GetDelayNextRun(mskTz, hour: 2, minute: 0);

            logger.LogInformation("Next run at 02:00 MSK. Sleeping {Delay}. Now (MSK): {Now}",
                delay, TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, mskTz));

            try
            {
                await Task.Delay(delay, stoppingToken);
            }
            catch (TaskCanceledException)
            {
                break;
            }

            try
            {
                await ImportTerminalsAsync(stoppingToken);
            }
            catch (Exception ex) when (!stoppingToken.IsCancellationRequested)
            {
                string message = $"Exception: {ex.Message}, InnerException: {ex.InnerException?.Message}";

                logger.LogError($"{DateTime.Now:dd.MM.yyyy HH:mm: ss} ERROR: {message}");
            }
        }
    }

    private async Task ImportTerminalsAsync(CancellationToken ct)
    {
        logger.LogInformation("Starting import at {Time}", DateTimeOffset.Now);

        try
        {
            string filePath = Path.GetFullPath(options.Value.TerminalsFilePath);
            RootDto root = TerminalParser.ParseJson(filePath);

            List<Office> offices = [];
            foreach (var city in root.City)
            {
                foreach (var term in city.Terminals?.Terminal ?? [])
                {
                    var office = mapper.Map<Office>(term);
                    office.CityCode = city.CityID;
                    office.AddressCity = city.Name;

                    offices.Add(office);
                }
            }
            int count = offices.Count;

            logger.LogInformation($"{DateTime.Now:dd.MM.yyyy HH:mm:ss} INFO: Загружено {count} терминалов из JSON");

            await using var context = await contextFactory.CreateDbContextAsync(ct);

            int oldCount = await context.Offices.CountAsync(ct);
            await context.Offices.ExecuteDeleteAsync(ct);

            logger.LogInformation($"{DateTime.Now:dd.MM.yyyy HH:mm:ss} INFO: Удалено {oldCount} старых записей");

            context.Offices.AddRange(offices);
            await context.SaveChangesAsync(ct);

            int newCount = await context.Offices.CountAsync(ct);

            logger.LogInformation($"{DateTime.Now:dd.MM.yyyy HH:mm:ss} INFO: Сохранено {newCount} новых терминалов");
        }
        catch (Exception ex)
        {
            string message = $"Exception: {ex.Message}, InnerException: {ex.InnerException?.Message}";

            logger.LogError($"{ DateTime.Now:dd.MM.yyyy HH:mm: ss} ERROR: {message}");
        }
    }

    private static TimeZoneInfo GetMoscowTimeZone()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            return TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time");

        return TimeZoneInfo.FindSystemTimeZoneById("Europe/Moscow");
    }

    private static TimeSpan GetDelayNextRun(TimeZoneInfo tz, int hour, int minute)
    {
        var nowUtc = DateTimeOffset.UtcNow;
        var nowLocal = TimeZoneInfo.ConvertTime(nowUtc, tz);

        var nextLocal = new DateTimeOffset(
            nowLocal.Year, nowLocal.Month, nowLocal.Day,
            hour, minute, 0,
            nowLocal.Offset);

        if (nextLocal <= nowLocal)
            nextLocal = nextLocal.AddDays(1);

        var nextUtc = TimeZoneInfo.ConvertTime(nextLocal, TimeZoneInfo.Utc);
        return nextUtc - nowUtc;
    }
}
