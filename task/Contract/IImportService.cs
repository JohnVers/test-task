using task.Entities;

namespace task.Contract;

public interface IImportService
{
    Task ImportAsync(IList<Office> offices, CancellationToken cancellationToken);
}
