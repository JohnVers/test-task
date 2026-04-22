using Api.Models;
using MediatR;

namespace Api.Queries
{
    public class SearchByCityQuery : IRequest<List<SearchByCityViewModel>>
    {
        public string CityName { get; set; }
        public string? Region { get; set; }
    }
}
