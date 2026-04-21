namespace task.Interfaces;
public interface IImportService
{
	Task ImportDataAsync(CancellationToken cancellationToken);
};