using Microsoft.AspNetCore.Mvc;
using task.Services;

namespace task.Controllers
{
    [Route("api")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly TerminalsService _terminalsService;

        public ApiController(TerminalsService terminalsService)
        {
            _terminalsService = terminalsService;
        }

        /// <summary>Поиск терминалов по названию города и области</summary>
        /// <param name="cityName">Название города</param>
        /// <param name="region">Название области (опционально)</param>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpGet("terminals")]
        public async Task<IActionResult> GetTerminalsByCity(
            [FromQuery] string cityName,
            [FromQuery] string? region = null,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(cityName))
                return BadRequest("Название города обязательно");

            var result = await _terminalsService.GetTerminalsByCityAsync(cityName, region, cancellationToken);
            return Ok(result);
        }

        /// <summary>Поиск идентификатора города по названию города и области</summary>
        /// <param name="cityName">Название города</param>
        /// <param name="region">Название области (опционально)</param>
        /// <param name="cancellationToken">Токен отмены</param>
        [HttpGet("terminals/city-id")]
        public async Task<IActionResult> GetCityId(
            [FromQuery] string cityName,
            [FromQuery] string? region = null,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(cityName))
                return BadRequest("Название города обязательно");

            var cityCode = await _terminalsService.GetCityIdAsync(cityName, region, cancellationToken);

            if (cityCode is null)
                return NotFound($"Город '{cityName}' не найден");

            return Ok(cityCode);
        }
    }
}
