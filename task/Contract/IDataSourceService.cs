using task.Entities;

namespace task.Contract;

public interface IDataSourceService
{
    Task<IList<Office>> LoadAsync(string filePath, CancellationToken cancellationToken);
}
