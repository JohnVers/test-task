using Application.Services.ImportTerminalsService.Interfaces;
using Microsoft.Extensions.Options;
using Quartz;
using task.Options;

namespace task.Jobs;

/// <summary>
/// Работа по импорту терминалов
/// </summary>
public class ImportTerminalsJob : IJob
{
    private readonly ILogger<ImportTerminalsJob> _logger;
    private readonly IImportTerminalsService _importTerminalsService;
    private readonly IServiceProvider _serviceProvider;

    public ImportTerminalsJob(ILogger<ImportTerminalsJob> logger,
        IImportTerminalsService importTerminalsService,
        IServiceProvider serviceProvider)
    {
        _logger = logger;
        _importTerminalsService = importTerminalsService;
        _serviceProvider = serviceProvider;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("ImportTerminalsJob начал работу в {Time}.", DateTime.Now);

        var fileSettings = _serviceProvider.GetRequiredService<IOptions<FileSettings>>().Value;
        
        var filePath = Path.Combine(AppContext.BaseDirectory, fileSettings.FilePath);

        await _importTerminalsService.ImportAsync(filePath, context.CancellationToken);

        _logger.LogInformation("ImportTerminalsJob завершил работу в {Time}.", DateTime.Now);
    }
}