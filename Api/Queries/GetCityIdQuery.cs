using MediatR;

namespace Api.Queries
{
    public class GetCityIdQuery : IRequest<int>
    {
        public string CityName { get; set; }
        public string? Region { get; set; }
    }
}
