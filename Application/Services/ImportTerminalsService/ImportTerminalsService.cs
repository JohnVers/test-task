using Application.Helpers;
using Application.Services.ImportTerminalsService.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistence.Common;

namespace Application.Services.ImportTerminalsService;

/// <inheritdoc/>
public class ImportTerminalsService : IImportTerminalsService
{
    private readonly ILogger<ImportTerminalsService> _logger;
    private readonly DellinDictionaryDbContext _dbContext;

    public ImportTerminalsService(DellinDictionaryDbContext dbContext,
        ILogger<ImportTerminalsService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task ImportAsync(string filePath, CancellationToken cancellationToken)
    {
        try
        {
            var root = TerminalParser.Parse(filePath);

            var terminals = root.Cities.SelectMany(city => city.GetTerminals());
            
            _logger.LogInformation("Загружено {Count} терминалов из JSON", terminals.Count());
            
            await ImportToDatabaseAsync(terminals, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка импорта: {Exception}", ex.Message);
        }
    }
    
    private async Task ImportToDatabaseAsync(IEnumerable<Office> terminals, CancellationToken cancellationToken)
    {
        try
        {
            var oldOfficesCount = await _dbContext.Offices.CountAsync(cancellationToken);

            await _dbContext.Offices.ExecuteDeleteAsync(cancellationToken);
            
            _logger.LogInformation("Удалено {OldCount} старых записей", oldOfficesCount);

            _dbContext.Offices.AddRange(terminals);
            await _dbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Сохранено {NewCount} новых терминалов", terminals.Count());
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Ошибка сохранения данных в БД: {Exception}", ex.Message);
            throw;
        }
    }
}