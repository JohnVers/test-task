namespace task.Application.Abstractions;

public interface ITerminalImportService
{
    Task ImportAsync(CancellationToken cancellationToken);
}
