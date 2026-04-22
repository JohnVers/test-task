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
            await DoWorkAsync(importTimeOut, logger, stoppingToken);
            await ForNextRunAsync(stoppingToken);
        }
    }

    private async Task DoWorkAsync(TimeSpan importTimeOut, ILogger<Worker> logger, CancellationToken cancellationToken)
    {
        logger.LogInformation("{Time} Импорт запущен", DateTimeOffset.Now);

        IList<Office>? offices = null;

        try
        {
            logger.LogInformation("{Time} Загрузка данных из JSON", DateTimeOffset.Now);

            offices = await dataSourceService.LoadAsync(options.Value.FilePath, cancellationToken);

            logger.LogInformation("{Time} Загружено {Count} терминалов из JSON", DateTimeOffset.Now, offices.Count);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            logger.LogInformation("{Time} Служба остановлена", DateTimeOffset.Now);
        }
        catch (Exception ex)
        {
            logger.LogCritical("{Time} Ошибка загрузки терминалов из JSON: {ex}", DateTimeOffset.Now, ex);

            return;
        }

        try
        {
            logger.LogInformation("{Time} Запись в БД", DateTimeOffset.Now);

            using (var timeoutSource = new CancellationTokenSource(importTimeOut))
            {
                using (var linkedSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeoutSource.Token))
                {
                    await importService.ImportAsync(offices ?? Array.Empty<Office>(), linkedSource.Token);
                }
            }

            logger.LogInformation("{Time} Запись в БД завершена", DateTimeOffset.Now);
        }
        catch (TimeoutException)
        {
            logger.LogError("{Time} Прeвышено время выполнения импорта ({TimeOut})", DateTimeOffset.Now, importTimeOut);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            logger.LogInformation("{Time} Служба остановлена", DateTimeOffset.Now);
        }
        catch (Exception ex)
        {
            logger.LogError("{Time} Ошибка импорта: {Exception}", DateTimeOffset.Now, ex);
        }

        logger.LogInformation("{Time} Импорт завершен", DateTimeOffset.Now);
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
