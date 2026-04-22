using Api.Models;
using Api.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Queries.SearchByCity
{
    public class SearchByCityHandler : IRequestHandler<SearchByCityQuery, List<SearchByCityViewModel>>
    {
        private readonly DellinDictionaryDbContext _dbContext;
        private readonly ILogger<SearchByCityHandler> _logger;

        public SearchByCityHandler(DellinDictionaryDbContext dbContext, ILogger<SearchByCityHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<List<SearchByCityViewModel>> Handle(SearchByCityQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.CityName))
                throw new Exception("City name is required");

            _logger.LogInformation("Searching terminals by city: {CityName}, region: {Region}", request.CityName, request.Region ?? "any");

            var query = _dbContext.Offices
                .Include(x => x.Phones)
                .AsNoTracking();

            query = query.Where(x =>
                EF.Functions.ILike(x.AddressCity ?? "", $"%{request.CityName}%"));

            if (!string.IsNullOrWhiteSpace(request.Region))
            {
                query = query.Where(x =>
                    EF.Functions.ILike(x.AddressRegion ?? "", $"%{request.Region}%"));
            }

            var results = await query.ToListAsync(cancellationToken);

            _logger.LogInformation("Found {Count} terminals", results.Count);

            var result = new List<SearchByCityViewModel>();

            foreach (var item in results)
            {
                var resultItem = new SearchByCityViewModel()
                { 
                    Id = item.Id,
                    AddressCity = item.AddressCity ?? "",
                    AddressHouseNumber = item.AddressHouseNumber ?? "",
                    AddressRegion = item.AddressRegion ?? "",
                    AddressStreet = item.AddressStreet ?? "",
                    Code = item.Code ?? "",
                    Type = item.Type.ToString() ?? "",
                    Phones = item.Phones.Select(p => p.PhoneNumber).ToList(),
                    Coordinates = new CoordinatesViewModel()
                    { 
                        Latitude = item.Latitude,
                        Longitude = item.Longitude
                    }
                };
                result.Add(resultItem);
            }

            return result;
        }
    }
}
