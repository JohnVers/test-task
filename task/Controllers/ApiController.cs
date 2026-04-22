using Api.Models;
using Api.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace task.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ApiController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Поиск терминалов по названию города и области
        /// </summary>
        /// <param name="query">Модель запроса</param>
        /// <returns>Список терминалов</returns>
        [HttpGet("SearchByCity")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<SearchByCityViewModel>>> SearchByCity([FromQuery] SearchByCityQuery query)
        {
            return await _mediator.Send(query);
        }

        /// <summary>
        /// Поиск идентификатора города по названию города и области
        /// </summary>
        /// <param name="query">Модель запроса</param>
        /// <returns>Идентификатор города</returns>
        [HttpGet("GetCityId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<int>> GetCityId([FromQuery] GetCityIdQuery query)
        {
            return await _mediator.Send(query);
        }
    }
}
