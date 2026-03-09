using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Polly;
using Polly.Retry;
using StranaKart.Assignment.Domain.Entities;
using StranaKart.Assignment.Infrastructure;

namespace StranaKart.Assignment.Services;

internal class TerminalSynchronizationService : ITerminalSynchronizationService
{
    private readonly ITerminalProviderService _terminalProviderService;
    private readonly DellinDictionaryDbContext _dbContext;
    private readonly ILogger<TerminalSynchronizationService> _logger;

    private readonly AsyncRetryPolicy _retryPolicy;
    private const int RetryCount = 3;
    private const int RetryBaseDelaySeconds = 2;

    public TerminalSynchronizationService(ITerminalProviderService terminalProviderService,
        DellinDictionaryDbContext dbContext, ILogger<TerminalSynchronizationService> logger)
    {
        _terminalProviderService = terminalProviderService;
        _dbContext = dbContext;
        _logger = logger;

        _retryPolicy = BuildRetryPolicy(_logger);
    }

    public async Task SyncOfficesAsync(CancellationToken ct)
    {
        var incomingOffices = await _terminalProviderService.GetOfficesAsync(ct);
        var officesToImport = PrepareOfficesForImport(incomingOffices);

        if (officesToImport.Count == 0)
        {
            _logger.LogWarning("Входящий список терминалов пуст — синхронизация отменена");
            return;
        }
        
        _logger.LogInformation("Подготовлено {Count} терминалов к импорту", officesToImport.Count);

        await _retryPolicy.ExecuteAsync(async token =>
        {
            _dbContext.ChangeTracker.Clear();

            await using var transaction = await _dbContext.Database.BeginTransactionAsync(token);

            var deletedCount = await _dbContext.Offices.ExecuteDeleteAsync(token);
            _dbContext.Offices.AddRange(officesToImport);
            await _dbContext.SaveChangesAsync(token);

            await transaction.CommitAsync(token);

            _logger.LogInformation(
                "Синхронизация завершена. Удалено: {Deleted}, Добавлено: {Added}",
                deletedCount, officesToImport.Count);
        }, ct);
    }

    private List<Office> PrepareOfficesForImport(IEnumerable<Office> incomingOffices)
    {
        var officesById = new Dictionary<int, Office>();
        var duplicates = 0;

        foreach (var office in incomingOffices)
        {
            if (!officesById.TryAdd(office.Id, office))
            {
                officesById[office.Id] = office;
                duplicates++;
            }
        }

        if (duplicates > 0)
            _logger.LogWarning("Найдено {Count} дублей по Id во входном файле. Оставлена последняя запись для каждого Id", duplicates);

        foreach (var office in officesById.Values)
        {
            office.Uuid ??= Guid.NewGuid();
        }

        return officesById.Values.ToList();
    }

    private static AsyncRetryPolicy BuildRetryPolicy(ILogger logger)
    {
        return Policy.Handle<DbException>(ex => ex.IsTransient)
            .Or<DbUpdateException>(ex => ex.InnerException is DbException { IsTransient: true })
            .Or<TimeoutException>()
            .WaitAndRetryAsync(
                retryCount: RetryCount,
                sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(RetryBaseDelaySeconds, attempt)),
                onRetry: (exception, delay, attempt, _) =>
                    logger.LogWarning(exception, "Повторная попытка ({RetryAttempt}) через {RetryDelay}с",
                        attempt, delay.TotalSeconds)
            );
    }
}
