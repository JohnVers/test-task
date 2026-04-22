using task.Entities;

namespace task.Contract;

/// <summary>
/// Сервис импорта 
/// </summary>
public interface IImportService
{
    /// <summary>
    /// Импортирует записи в базу данных
    /// </summary>
    Task ImportAsync(IList<Office> offices, CancellationToken cancellationToken);
}
