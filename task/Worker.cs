using task.Services;

namespace task;

public class Worker : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<Worker> _logger;

    public Worker(IServiceScopeFactory scopeFactory, ILogger<Worker> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await RunImportAsync(stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            var delay = GetDelayUntilNextRun();
            _logger.LogInformation("Следующий импорт запланирован через {Delay:hh\\:mm\\:ss}", delay);

            try
            {
                await Task.Delay(delay, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                break;
            }

            if (!stoppingToken.IsCancellationRequested)
                await RunImportAsync(stoppingToken);
        }
    }

    private async Task RunImportAsync(CancellationToken stoppingToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var importService = scope.ServiceProvider.GetRequiredService<TerminalsImportService>();
        try
        {
            await importService.ImportAsync(stoppingToken);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            _logger.LogError(ex, "Критическая ошибка при выполнении импорта терминалов");
        }
    }

    private static TimeSpan GetDelayUntilNextRun()
    {
        var mskZone = GetMskTimeZone();
        var nowUtc = DateTime.UtcNow;
        var nowMsk = TimeZoneInfo.ConvertTimeFromUtc(nowUtc, mskZone);

        var nextRunMsk = nowMsk.Date.AddHours(2);
        if (nowMsk >= nextRunMsk)
            nextRunMsk = nextRunMsk.AddDays(1);

        var nextRunUtc = TimeZoneInfo.ConvertTimeToUtc(nextRunMsk, mskZone);
        var delay = nextRunUtc - nowUtc;
        return delay > TimeSpan.Zero ? delay : TimeSpan.Zero;
    }

    private static TimeZoneInfo GetMskTimeZone()
    {
        try 
        { 
            return TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time"); 
        }
        catch 
        { 
            return TimeZoneInfo.FindSystemTimeZoneById("Europe/Moscow"); 
        }
    }
}
