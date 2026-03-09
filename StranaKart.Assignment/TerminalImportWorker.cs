using System.Diagnostics;
using StranaKart.Assignment.Infrastructure.Scheduling;
using StranaKart.Assignment.Services;

namespace StranaKart.Assignment;

internal class TerminalImportWorker : BackgroundService
{
    private static readonly TimeSpan FallbackScheduleDelay = TimeSpan.FromMinutes(1);

    private readonly IScheduleService _scheduleService;
    private readonly ILogger<TerminalImportWorker> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public TerminalImportWorker(IScheduleService scheduleService,
        ILogger<TerminalImportWorker> logger,
        IServiceScopeFactory serviceScopeFactory)
    {
        _scheduleService = scheduleService;
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var _ = _logger.BeginScope(new Dictionary<string, object>
        {
            ["Worker"] = nameof(TerminalImportWorker)
        });
        
        _logger.LogInformation("Запуск воркера {Worker}", nameof(TerminalImportWorker));
        
        // Первый запуск выполняется немедленно - намеренно для простоты тестирования
        while (!stoppingToken.IsCancellationRequested)
        {
            var importId = Guid.NewGuid();

            using (_logger.BeginScope(new Dictionary<string, object> { { "ImportId", importId } }))
            {
                try
                {
                    using var scope = _serviceScopeFactory.CreateScope();
                    _logger.LogInformation("Запуск импорта {ImportId}", importId);

                    var syncService = scope.ServiceProvider.GetRequiredService<ITerminalSynchronizationService>();

                    var stopWatch = Stopwatch.StartNew();
                    await syncService.SyncOfficesAsync(stoppingToken);
                    stopWatch.Stop();

                    _logger.LogInformation("Импорт {ImportId} успешно завершён. Время выполнения: {Elapsed}",
                        importId, stopWatch.Elapsed);
                }
                catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                {
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Ошибка обработки итерации импорта {ImportId}", importId);
                }
            }

            TimeSpan nextDelay;
            try
            {
                var nextRunInfo = _scheduleService.GetNextRunInfo();
                nextDelay = CalculateDelay(nextRunInfo);

                _logger.LogInformation("Следующий запуск назначен на {NextRun:dd.MM.yyyy HH:mm} ({TimeZone})",
                    nextRunInfo.NextRun, nextRunInfo.TimeZone);
            }
            catch (Exception ex)
            {
                nextDelay = FallbackScheduleDelay;
                _logger.LogError(ex,
                    "Не удалось рассчитать следующий запуск. Будет использована резервная задержка {Delay}",
                    FallbackScheduleDelay);
            }

            try
            {
                await Task.Delay(nextDelay, stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
        }

        _logger.LogInformation("Остановка воркера {Worker}",  nameof(TerminalImportWorker));
    }

    private static TimeSpan CalculateDelay(Domain.Scheduling.NextRunInfo nextRunInfo)
    {
        var timeZone = TimeZoneInfo.FindSystemTimeZoneById(nextRunInfo.TimeZone);
        var localNow = TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, timeZone);
        var nextRun = new DateTimeOffset(nextRunInfo.NextRun, timeZone.GetUtcOffset(nextRunInfo.NextRun));
        var delay = nextRun - localNow;

        return delay > TimeSpan.Zero ? delay : TimeSpan.FromSeconds(1);
    }
}
