using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using task.Data;
using task.Interfaces;
using task.JsonModels;
using task.Models;

namespace task.Services;

public class ImportService(DellinDictionaryDbContext dbContext,
		IConfiguration configuration,
		ILogger<ImportService> logger) : IImportService
{
	private readonly string _jsonFilePath = configuration.GetValue<string>("ImportSettings:JsonFilePath")
						 ?? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "files", "terminals.json");

	private Office MapToOffice(CityJson city, TerminalJson terminal)
	{
		var type = terminal switch
		{
			{ IsPVZ: true } => OfficeType.PVZ,
			{ IsOffice: true } => OfficeType.OFFICE,
			{ Storage: true } => OfficeType.WAREHOUSE,
			_ => OfficeType.UNWKOWN
		};

		Coordinates coordinates = null!;

		if (!string.IsNullOrEmpty(terminal.Latitude) && !string.IsNullOrEmpty(terminal.Longitude) &&
			double.TryParse(terminal.Latitude, out var lat) && double.TryParse(terminal.Longitude, out var lon))
		{
			coordinates = new Coordinates { Latitude = lat, Longitude = lon };
		}

		var phoneSet = new HashSet<string>();
		var phones = new List<Phone>();

		if (terminal.Phones != null)
		{
			foreach (var p in terminal.Phones)
			{
				if (!string.IsNullOrWhiteSpace(p.Number) && phoneSet.Add(p.Number))
				{
					phones.Add(new Phone { PhoneNumber = p.Number, Additional = p.Comment });
				}
			}
		}

		if (!string.IsNullOrWhiteSpace(terminal.MainPhone) && phoneSet.Add(terminal.MainPhone))
		{
			phones.Add(new Phone { PhoneNumber = terminal.MainPhone });
		}

		JsonElement workTime = terminal.Worktables.Clone();

		return new Office
		{
			CountryCode = "RU",
			Code = city.Code,
			CityCode = city.CityID!.Value,
			Uuid = terminal.Id,
			AddressCity = city.Name,
			AddressStreet = terminal.Address,
			Type = type,
			WorkTime = workTime,
			Coordinates = coordinates,
			Phones = phones
		};
	}

	public async Task ImportDataAsync(CancellationToken cancellationToken)
	{
		logger.LogInformation("Запуск импорта терминалов из файла {FilePath}", _jsonFilePath);
		var startTime = DateTime.UtcNow;

		try
		{
			if (!File.Exists(_jsonFilePath))
			{
				logger.LogError("Файл {FilePath} не найден", _jsonFilePath);
				return;
			}

			await using var fileStream = File.OpenRead(_jsonFilePath);

			var root = await JsonSerializer.DeserializeAsync<TerminalsRoot>(
				fileStream,
				new JsonSerializerOptions { PropertyNameCaseInsensitive = true },
				cancellationToken);

			if (root?.City == null)
			{
				logger.LogWarning("JSON не содержит городов или имеет неверную структуру");
				return;
			}

			var offices = new List<Office>();

			foreach (var city in root.City)
			{
				// Пропускаем город, если cityID == null или terminals отсутствует/пуст
				if (city.CityID == null || city.Terminals?.Terminal == null || city.Terminals.Terminal.Count == 0)
				{
					logger.LogWarning("Пропущен город {CityName}: cityID={CityID}, terminals={TerminalsCount}",
						city.Name, city.CityID, city.Terminals?.Terminal?.Count ?? 0);
					continue;
				}

				foreach (var terminal in city.Terminals.Terminal)
				{
					var office = MapToOffice(city, terminal);
					offices.Add(office);
				}
			}

			logger.LogInformation("Загружено {Count} записей из JSON", offices.Count);

			// Очистка таблицы (эффективный DELETE)
			int rowsDeleted = await dbContext.Offices.ExecuteDeleteAsync(cancellationToken);

			logger.LogInformation("Удалено {OldCount} старых записей", rowsDeleted);

			// Массовая вставка
			await dbContext.Offices.AddRangeAsync(offices, cancellationToken);
			await dbContext.SaveChangesAsync(cancellationToken);

			var elapsed = DateTime.UtcNow - startTime;

			logger.LogInformation("Импорт завершён. Вставлено {Count} записей за {Elapsed} сек", offices.Count, elapsed.TotalSeconds);
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "Ошибка при импорте данных");
		}
	}
}
