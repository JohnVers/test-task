using Microsoft.AspNetCore.Mvc;
using task.Contract;

namespace task.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class ApiTestController(IService service) : ControllerBase
{
    /// <summary>
    /// Поиск терминалов города по названию города и области
    /// Не совсем понятно какое поле в json содержит область так что в базе cityName = regionName 
    /// </summary>
    /// <param name="cityName">Название города</param>
    /// <param name="regionName">Название области, не совсем понятно какое поле в json содержит область так что в базе cityName = regionName</param>
    [HttpGet("city")]
    public async Task<IActionResult> GetByCityNameAsync(string cityName, string regionName, CancellationToken cancellationToken)
    {
        var result = await service.GetOfficesAsync(cityName, regionName, cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// Поиск идентификатора города по названию города и области
    /// </summary>
    /// <param name="cityName">Название города</param>
    /// <param name="regionName">Название области, не совсем понятно какое поле в json содержит область так что в базе cityName = regionName</param>
    /// <returns></returns>
    [HttpGet("region")]
    public async Task<IActionResult> GetByCityIdRegionNameAsync(string cityName, string regionName, CancellationToken cancellationToken)
    {
        var result = await service.GetCityIdAsync(cityName, regionName, cancellationToken);

        return Ok(result);
    }
}
