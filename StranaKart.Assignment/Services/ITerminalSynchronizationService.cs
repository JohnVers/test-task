namespace StranaKart.Assignment.Services;

public interface ITerminalSynchronizationService
{
    Task SyncOfficesAsync(CancellationToken ct);
}