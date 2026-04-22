using Api.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Queries.GetCityId
{
    public class GetCityIdHandler : IRequestHandler<GetCityIdQuery, int>
    {
        private readonly DellinDictionaryDbContext _dbContext;
        private readonly ILogger<GetCityIdHandler> _logger;

        public GetCityIdHandler(DellinDictionaryDbContext dbContext, ILogger<GetCityIdHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        public async Task<int> Handle(GetCityIdQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.CityName))
                throw new Exception("City name is required");

            _logger.LogInformation("Searching city ID by city: {CityName}, region: {Region}", request.CityName, request.Region ?? "any");

            var query = _dbContext.Offices
                .AsNoTracking()
                .Where(x => EF.Functions.ILike(x.AddressCity ?? "", $"%{request.CityName}%"));

            if (!string.IsNullOrWhiteSpace(request.Region))
            {
                query = query.Where(x =>
                    EF.Functions.ILike(x.AddressRegion ?? "", $"%{request.Region}%"));
            }

            var office = await query.FirstOrDefaultAsync(cancellationToken);

            if (office == null)
            {
                _logger.LogWarning("City not found: {CityName}", request.CityName);
                throw new Exception($"City '{request.CityName}' not found" );
            }

            return office.CityCode;
        }
    }
}
