using Quartz;

namespace task.Jobs;

/// <summary>
/// Работа по импорту терминалов
/// </summary>
public class ImportTerminalsJob : IJob
{
    private readonly  ILogger<ImportTerminalsJob> _logger;

    public ImportTerminalsJob(ILogger<ImportTerminalsJob> logger)
    {
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("ImportTerminalsJob начал работу в {Time}.", DateTime.Now);

        await Task.Delay(1000); // 

        _logger.LogInformation("ImportTerminalsJob завершил работу в {Time}.", DateTime.Now);
    }
}