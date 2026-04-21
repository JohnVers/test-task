using task.Interfaces;

namespace task;

public class Worker(IServiceScopeFactory scopeFactory,
		IConfiguration configuration,
		ILogger<Worker> logger) : BackgroundService
{
	private readonly TimeOnly _importTime = ParseImportTime(configuration);

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		while (!stoppingToken.IsCancellationRequested)
		{
			var now = DateTime.Now;
			var nextRun = now.Date.Add(_importTime.ToTimeSpan());
			
			if (now >= nextRun)
				nextRun = nextRun.AddDays(1);

			var delay = nextRun - now;
			logger.LogInformation("Следующий импорт запланирован на {NextRun} (через {Delay})", 
				nextRun.ToString("dd-MM-yyyy HH:mm:ss"), delay.ToString(@"hh\:mm\:ss"));

			await Task.Delay(delay, stoppingToken);

			if (!stoppingToken.IsCancellationRequested)
			{
				using var scope = scopeFactory.CreateScope();
				var importService = scope.ServiceProvider.GetRequiredService<IImportService>();
				await importService.ImportDataAsync(stoppingToken);
			}
		}
	}

	private static TimeOnly ParseImportTime(IConfiguration config)
	{
		var timeString = config.GetValue<string>("ImportSettings:ImportTime") ?? "02:00:00";
		
		if (TimeOnly.TryParse(timeString, out var time))
			return time;

		// Fallback
		return new TimeOnly(2, 0, 0);
	}
}
