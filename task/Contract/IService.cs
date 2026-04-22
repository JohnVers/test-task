using task.Entities;

namespace task.Contract;

public interface IService
{
    /// <summary>
    /// Возвращает список терминалов по имени города и области
    /// </summary>
    Task<IList<Office>> GetOfficesAsync(string cityName, string regionName, CancellationToken cancellationToken);

    /// <summary>
    /// Возвращает код города по имени города и области
    /// </summary>
    Task<int?> GetCityIdAsync(string cityName, string regionName, CancellationToken cancellationToken);
}
