using task.Application.Models;

namespace task.Application.Abstractions;

public interface IOfficeQueryService
{
    Task<IReadOnlyCollection<OfficeResponse>> FindOfficesByCityAndRegionAsync(string city, string? region, CancellationToken cancellationToken);

    Task<int?> FindCityCodeByCityAndRegionAsync(string city, string? region, CancellationToken cancellationToken);
}
