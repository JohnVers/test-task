using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using task.Application.Abstractions;
using task.Infrastructure.Import;
using task.Infrastructure.Persistence;
using task.Infrastructure.Utilities;

namespace task.Infrastructure.Services;

public sealed class TerminalImportService(TerminalsDictionaryDbContext dbContext, IWebHostEnvironment environment, ILogger<TerminalImportService> logger) : ITerminalImportService
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public async Task ImportAsync(CancellationToken cancellationToken)
    {
        var filePath = Path.Combine(environment.ContentRootPath, "files", "terminals.json");
        if (!File.Exists(filePath))
        {
            logger.LogWarning("Файл JSON не найден, импорт пропущен. Путь: {Path}", filePath);
            return;
        }

        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            await using var stream = File.OpenRead(filePath);
            var payload = await JsonSerializer.DeserializeAsync<TerminalDictionaryRoot>(stream, JsonOptions, cancellationToken);
            var offices = TerminalDictionaryMapper.MapToOffices(payload);

            logger.LogInformation("Загружено {Count} терминалов из JSON", offices.Count);

            var oldCount = await dbContext.Offices.CountAsync(cancellationToken);
            await dbContext.Phones.ExecuteDeleteAsync(cancellationToken);
            await dbContext.Offices.ExecuteDeleteAsync(cancellationToken);

            logger.LogInformation("Удалено {OldCount} старых записей", oldCount);

            dbContext.ChangeTracker.AutoDetectChangesEnabled = false;
            await dbContext.Offices.AddRangeAsync(offices, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
            dbContext.ChangeTracker.AutoDetectChangesEnabled = true;

            await transaction.CommitAsync(cancellationToken);

            logger.LogInformation("Сохранено {NewCount} новых терминалов", offices.Count);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            logger.LogError(ex, "Ошибка импорта: {Exception}", ex.Message);
            throw;
        }
    }
}
