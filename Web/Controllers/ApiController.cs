using Microsoft.AspNetCore.Mvc;
using task.Application.Abstractions;
using task.Web.Api.Models;

namespace task.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiController(IOfficeQueryService officeQueryService) : ControllerBase
    {
        [HttpGet("offices")]
        public async Task<IActionResult> FindOffices([FromQuery] CitySearchRequest request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.City))
                return BadRequest("Query parameter 'city' is required.");

            var offices = await officeQueryService.FindOfficesByCityAndRegionAsync(request.City, request.Region, cancellationToken);

            return Ok(offices);
        }

        [HttpGet("city-code")]
        public async Task<IActionResult> FindCityCode([FromQuery] CitySearchRequest request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.City))
                return BadRequest("Query parameter 'city' is required.");

            var cityCode = await officeQueryService.FindCityCodeByCityAndRegionAsync(request.City, request.Region, cancellationToken);
            if (cityCode is null)
                return NotFound();

            return Ok(new { CityCode = cityCode.Value });
        }
    }
}
