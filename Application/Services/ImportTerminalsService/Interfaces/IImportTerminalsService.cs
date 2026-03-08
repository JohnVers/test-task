namespace Application.Services.ImportTerminalsService.Interfaces;

/// <summary>
/// Сервис импорта терминалов
/// </summary>
public interface IImportTerminalsService
{
    Task ImportAsync(string filePath, CancellationToken cancellationToken);
}