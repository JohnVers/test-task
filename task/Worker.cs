using task.Application.Services.Interfaces;

namespace task;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;

    private readonly IConfigurationService _configurationService;
    private readonly IParsingService _parsingService;
    private readonly IMappingService _mappingService;
    private readonly IOfficeService _officeService;

    private readonly TimeSpan _executionTime;

    private const string DEFAULT_EXECUTION_TIME = "02:00:00";

    public Worker(ILogger<Worker> logger, IConfigurationService configurationService, IParsingService parsingService,
        IMappingService mappingService, IOfficeService officeService)
    {
        _logger = logger;
        _configurationService = configurationService;
        _parsingService = parsingService;
        _mappingService = mappingService;
        _officeService = officeService;

        _executionTime = TimeSpan.Parse(_configurationService.GetExecutionTime() ?? DEFAULT_EXECUTION_TIME);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var filePath = _configurationService.GetFileLocation();

        while (!stoppingToken.IsCancellationRequested)
        {
            var now = DateTime.Now;
            var nextRunTime = now.Date.Add(_executionTime);

            if (now > nextRunTime)
            {
                nextRunTime = nextRunTime.AddDays(1);
            }

            var delay = nextRunTime - now;
            

            await Task.Delay(delay, stoppingToken);

            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }

            try
            {
                var data = _parsingService.ParseData(filePath);

                var offices = _mappingService.MapRawData(data);

                var result = await _officeService.SaveData(offices, stoppingToken)
                        .ConfigureAwait(false);

                _logger.LogInformation($"Загружено {result.TotalTerminals} терминалов из JSON");
                _logger.LogInformation($"Удалено {result.DeletedCount} старых записей");
                _logger.LogInformation($"Сохранено {result.SavedCount} новых терминалов");

                if (!String.IsNullOrEmpty(result.ErrorMessage))
                {
                    _logger.LogInformation($"Загружено {result.TotalTerminals} терминалов из JSON");
                }

            }
            catch(Exception ex)
            {
                _logger.LogError(ex.InnerException?.Message ?? ex.Message);
            }
        }
    }
}
