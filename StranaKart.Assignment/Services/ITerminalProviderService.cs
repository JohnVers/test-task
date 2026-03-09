using StranaKart.Assignment.Domain.Entities;

namespace StranaKart.Assignment.Services;

public interface ITerminalProviderService
{
    public Task<IEnumerable<Office>> GetOfficesAsync(CancellationToken cancellationToken);
}