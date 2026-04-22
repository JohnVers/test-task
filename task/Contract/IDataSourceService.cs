using task.Entities;

namespace task.Contract;

/// <summary>
/// Сервис загрузки данных из JSON
/// </summary>
public interface IDataSourceService
{
    /// <summary>
    /// Возвращает коллекцию терминалов
    /// </summary>
    /// <param name="filePath">Путь к файлу источнику</param>
    Task<IList<Office>> LoadAsync(string filePath, CancellationToken cancellationToken);
}
