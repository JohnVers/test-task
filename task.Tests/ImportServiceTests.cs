using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using System.Text.Json;
using task.Data;
using task.JsonModels;
using task.Models;
using task.Services;

namespace task.Tests;

public class ImportServiceTests
{
	private readonly DellinDictionaryDbContext _dbContext;
	private readonly IConfiguration _configuration;
	private readonly ImportService _importService;
	private JsonElement _worktables = JsonDocument.Parse("""{"worktables":[{"timetable":"пн-пт 9:00-18:00"}]}""").RootElement;

	public ImportServiceTests()
	{
		// Используем InMemoryDatabase для тестов, чтобы не трогать реальную БД
		var options = new DbContextOptionsBuilder<DellinDictionaryDbContext>()
			.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
			.Options;
		_dbContext = new DellinDictionaryDbContext(options);

		var inMemorySettings = new Dictionary<string, string?>
		{
			{ "ImportSettings:JsonFilePath", "dummy.json" }
		};
		_configuration = new ConfigurationBuilder()
			.AddInMemoryCollection(inMemorySettings)
			.Build();

		_importService = new ImportService(
			_dbContext,
			_configuration,
			NullLogger<ImportService>.Instance);
	}

	private Office MapToOffice(CityJson city, TerminalJson terminal)
	{
		// Вызываем приватный метод через рефлексию или делаем его internal
		var method = typeof(ImportService).GetMethod("MapToOffice",
			System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

		return (Office)method!.Invoke(_importService, new object[] { city, terminal })!;
	}

	[Fact]
	public void MapToOffice_WithPVZFlag_ReturnsOfficeTypePVZ()
	{
		// Arrange
		var city = new CityJson { CityID = 123, Name = "Тестовый город" };
		var terminal = new TerminalJson
		{
			IsPVZ = true,
			Address = "ул. Тестовая, 1",
			Latitude = "55.0",
			Longitude = "37.0",
			Worktables = _worktables
		};

		// Act 
		var office = MapToOffice(city, terminal);

		// Assert
		office.Type.Should().Be(OfficeType.PVZ);
		office.CityCode.Should().Be(123);
		office.AddressCity.Should().Be("Тестовый город");
		office.AddressStreet.Should().Be("ул. Тестовая, 1");
	}

	[Fact]
	public void MapToOffice_WithStorageFlag_ReturnsWarehouse()
	{
		var city = new CityJson { CityID = 1, Name = "Складской" };
		var terminal = new TerminalJson
		{
			Storage = true,
			Worktables = _worktables
		};

		// Act 
		var office = MapToOffice(city, terminal);

		office.Type.Should().Be(OfficeType.WAREHOUSE);
	}

	[Fact]
	public void MapToOffice_WithOfficeFlag_ReturnsOffice()
	{
		var city = new CityJson { CityID = 1, Name = "Офис" };
		var terminal = new TerminalJson
		{
			IsOffice = true,
			Worktables = _worktables
		};

		// Act 
		var office = MapToOffice(city, terminal);

		office.Type.Should().Be(OfficeType.OFFICE);
	}

	[Fact]
	public void MapToOffice_WithoutFlags_ReturnsUnknown()
	{
		var city = new CityJson { CityID = 1, Name = "Не известный" };
		var terminal = new TerminalJson
		{
			IsOffice = false,
			IsPVZ = false,
			Storage = false,
			Worktables = _worktables
		};

		// Act 
		var office = MapToOffice(city, terminal);

		office.Type.Should().Be(OfficeType.UNWKOWN);
	}

	[Fact]
	public void MapToOffice_CollectsPhonesFromMainPhoneAndPhonesArray()
	{
		var city = new CityJson { CityID = 1, Name = "Тест" };
		var terminal = new TerminalJson
		{
			MainPhone = "+7 000 000-00-00",
			Phones = new List<PhoneJson>
			{
				new() { Number = "+7 111 111-11-11", Comment = "доп." },
				new() { Number = "+7 222 222-22-22" }
			},
			Worktables = _worktables
		};

		// Act 
		var office = MapToOffice(city, terminal);

		office.Phones.Should().HaveCount(3);
		office.Phones.Select(p => p.PhoneNumber).Should().Contain(new[]
		{
			"+7 000 000-00-00",
			"+7 111 111-11-11",
			"+7 222 222-22-22"
		});
	}

	[Fact]
	public void MapToOffice_WithWorktables_SetsWorkTimeAsJsonElement()
	{
		var city = new CityJson { CityID = 1, Name = "Тест" };
		var worktablesJson = """{"worktable":[{"timetable":"пн-пт 9:00-18:00"}]}""";
		var element = JsonDocument.Parse(worktablesJson).RootElement;
		var terminal = new TerminalJson { Worktables = element };

		// Act 
		var office = MapToOffice(city, terminal);

		office.WorkTime.Should().NotBeNull();
		office.WorkTime.GetRawText().Should().Be(worktablesJson);
	}
}