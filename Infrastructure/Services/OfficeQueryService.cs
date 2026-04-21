using Microsoft.EntityFrameworkCore;
using task.Application.Abstractions;
using task.Application.Models;
using task.Infrastructure.Persistence;

namespace task.Infrastructure.Services;

public sealed class OfficeQueryService(TerminalsDictionaryDbContext dbContext) : IOfficeQueryService
{
    public async Task<IReadOnlyCollection<OfficeResponse>> FindOfficesByCityAndRegionAsync(string city, string? region, CancellationToken cancellationToken)
    {
        IQueryable<Domain.Entities.Office> query = dbContext.Offices
            .AsNoTracking()
            .Include(x => x.Phones);
        query = WhereByCityAndRegion(query, city, region);

        var offices = await query
            .OrderBy(x => x.Id)
            .Select(x => new OfficeResponse(
                x.Id,
                x.Code,
                x.CityCode,
                x.Uuid,
                x.Type,
                x.CountryCode,
                x.Coordinates.Latitude,
                x.Coordinates.Longitude,
                x.AddressRegion,
                x.AddressCity,
                x.AddressStreet,
                x.AddressHouseNumber,
                x.AddressApartment,
                x.WorkTime,
                x.Phones
                    .Select(p => new PhoneResponse(p.PhoneNumber ?? string.Empty, p.Additional))
                    .ToArray()))
            .ToListAsync(cancellationToken);

        return offices;
    }

    public async Task<int?> FindCityCodeByCityAndRegionAsync(string city, string? region, CancellationToken cancellationToken)
    {
        var query = dbContext.Offices.AsNoTracking();
        query = WhereByCityAndRegion(query, city, region);

        return await query
            .Select(x => (int?)x.CityCode)
            .FirstOrDefaultAsync(cancellationToken);
    }

    private static IQueryable<Domain.Entities.Office> WhereByCityAndRegion(IQueryable<Domain.Entities.Office> query, string city, string? region)
    {
        var normalizedCity = city.Trim();
        var normalizedRegion = string.IsNullOrWhiteSpace(region) ? null : region.Trim();

        query = query.Where(x => x.AddressCity != null && EF.Functions.ILike(x.AddressCity, $"%{normalizedCity}%"));
        if (normalizedRegion is null)
        {
            return query;
        }

        return query.Where(x => x.AddressRegion != null && EF.Functions.ILike(x.AddressRegion, $"%{normalizedRegion}%"));
    }
}
