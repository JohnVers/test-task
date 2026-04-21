using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using task.Data;
using task.Models;

namespace task.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TerminalsController(DellinDictionaryDbContext context,
	ILogger<TerminalsController> logger) : ControllerBase
{
	/// <summary>
	/// Возвращает список офисов по названию города.
	/// Параметр region не используется, т.к. в исходных данных отсутствует.
	/// </summary>
	[HttpGet("by-city")]
	public async Task<ActionResult<IEnumerable<Office>>> GetByCity(
		[FromQuery] string cityName,
		[FromQuery] string? region = null)
	{
		if (string.IsNullOrWhiteSpace(cityName))
			return BadRequest("Не указано название города");

		// region игнорируется, но если передан не "RU", можно залогировать
		if (!string.IsNullOrEmpty(region) && !region.Equals("RU", StringComparison.OrdinalIgnoreCase))
		{
			logger.LogWarning("Запрос с неизвестным регионом: {Region}", region);
		}

		var offices = await context.Offices
			.Where(o => o.AddressCity == cityName)
			.Include(o => o.Phones)
			.AsNoTracking()
			.ToListAsync();

		if (offices.Count == 0)
			return NotFound($"Офисы в городе '{cityName}' не найдены");

		return Ok(offices);
	}

	/// <summary>
	/// Возвращает код города по названию города.
	/// Параметр region не используется, т.к. в исходных данных отсутствует.
	/// </summary>
	[HttpGet("city-id")]
	public async Task<ActionResult<int>> GetCityId(
		[FromQuery] string cityName,
		[FromQuery] string? region = null)
	{
		if (string.IsNullOrWhiteSpace(cityName))
			return BadRequest("Не указано название города");

		var cityCode = await context.Offices
			.Where(o => o.AddressCity == cityName)
			.Select(o => o.CityCode)
			.FirstOrDefaultAsync();

		if (cityCode == default)
			return NotFound($"Код города '{cityName}' не найден");

		return Ok(cityCode);
	}

	/// <summary>
	/// Возвращает список офисов по коду города.
	/// </summary>
	[HttpGet("by-city-id/{cityId:int}")]
	public async Task<ActionResult<IEnumerable<Office>>> GetByCityId(int cityId)
	{
		var offices = await context.Offices
			.Where(o => o.CityCode == cityId)
			.Include(o => o.Phones)
			.AsNoTracking()
			.ToListAsync();

		if (offices.Count == 0)
			return NotFound($"Офисы с кодом города '{cityId}' не найдены");

		return Ok(offices);
	}
}
