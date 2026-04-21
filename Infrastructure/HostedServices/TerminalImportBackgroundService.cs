using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using task.Application.Abstractions;
using task.Infrastructure.Utilities;

namespace task.Infrastructure.HostedServices;

public sealed class TerminalImportBackgroundService(IServiceScopeFactory scopeFactory, ILogger<TerminalImportBackgroundService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Фоновая служба импорта терминалов запущена");

        await ImportWithScopeAsync(stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            var delay = MoscowScheduling.GetDelayUntilNextDailyRunAtTwoAmMsk(DateTimeOffset.UtcNow);
            logger.LogInformation("Следующий импорт запланирован через {Delay}", delay);

            try
            {
                await Task.Delay(delay, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                break;
            }

            await ImportWithScopeAsync(stoppingToken);
        }

        logger.LogInformation("Фоновая служба импорта терминалов остановлена");
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Корректная остановка фоновой службы импорта терминалов");
        await base.StopAsync(cancellationToken);
    }

    private async Task ImportWithScopeAsync(CancellationToken cancellationToken)
    {
        using var scope = scopeFactory.CreateScope();
        var importer = scope.ServiceProvider.GetRequiredService<ITerminalImportService>();

        try
        {
            await importer.ImportAsync(cancellationToken);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            logger.LogInformation("Импорт отменён");
        }
        catch (Exception)
        {
            logger.LogError("Импорт завершился с ошибкой. Подробности см. в логах сервиса импорта.");
        }
    }

}
