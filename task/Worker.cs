using System.Runtime.InteropServices;
using Microsoft.Extensions.Options;
using task.Contract;
using task.Entities;
using task.Options;

namespace task;

internal class Worker(IOptions<ImportOptions> options,
    IDataSourceService dataSourceService,
    IImportService importService,
    ILogger<Worker> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

        var importTimeOut = TimeSpan.FromMinutes(options.Value.TimeOut);

        while (!stoppingToken.IsCancellationRequested)
        {
            await ForNextRunAsync(stoppingToken);

            logger.LogInformation("{Time} Импорт запущен", DateTimeOffset.Now);

            IList<Office>? offices = null;

            try
            {
                logger.LogInformation("{Time} Загрузка данных из JSON", DateTimeOffset.Now);

                offices = await dataSourceService.LoadAsync(options.Value.FilePath, stoppingToken);

                logger.LogInformation("{Time} Загружено {Count} терминалов из JSON", DateTimeOffset.Now, offices.Count);
            }
            catch (TaskCanceledException)
            {
                logger.LogInformation("{Time} Загрузка терминалов из JSON отменена", DateTimeOffset.Now);
            }
            catch (Exception ex)
            {
                logger.LogInformation("{Time} Ошибка загрузки терминалов из JSON: {ex}", DateTimeOffset.Now, ex);

                continue;
            }

            try
            {
                logger.LogInformation("{Time} Запись в БД", DateTimeOffset.Now);

                var task = importService.ImportAsync(offices ?? Array.Empty<Office>(), stoppingToken);
                await task.WaitAsync(importTimeOut, stoppingToken);

                logger.LogInformation("{Time} Запись в БД завершнена", DateTimeOffset.Now);
            }
            catch (TimeoutException)
            {
                logger.LogError("{Time} Привышено время выполнения импорта ({TimeOut})", DateTimeOffset.Now, importTimeOut);
            }
            catch (TaskCanceledException)
            {
                logger.LogInformation("Служба остановлена");
            }
            catch (Exception ex)
            {
                logger.LogError("{Time} Ошибка импорта: {Exception}", DateTimeOffset.Now, ex);
            }

            logger.LogInformation("{Time} Импорт завершен", DateTimeOffset.Now);
        }
    }

    private static async Task ForNextRunAsync(CancellationToken cancellationToken)
    {
        var moscowTimeZone = GetTimeZone();
        var nowUtc = DateTime.UtcNow;
        var moscowTime = TimeZoneInfo.ConvertTimeFromUtc(nowUtc, moscowTimeZone);
        var moscowRunTime = new DateTime(moscowTime.Year, moscowTime.Month, moscowTime.Day, 02, 0, 0);

        moscowRunTime = moscowRunTime < moscowTime ? moscowRunTime.AddDays(1) : moscowRunTime;

        var nextRunUtc = TimeZoneInfo.ConvertTimeToUtc(moscowRunTime, moscowTimeZone);
        var delay = (nextRunUtc - nowUtc).Duration();

        await Task.Delay(delay, cancellationToken);
    }

    private static TimeZoneInfo GetTimeZone()
    {
        var id = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
            ? "Russian Standard Time"
            : "Europe/Moscow";

        return TimeZoneInfo.FindSystemTimeZoneById(id);
    }
}
