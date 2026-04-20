using Microsoft.EntityFrameworkCore;
using task.Data;
using task.Models;

namespace task.Services;

public class TerminalsService
{
    private readonly DellinDictionaryDbContext _context;

    public TerminalsService(DellinDictionaryDbContext context)
    {
        _context = context;
    }

    public async Task<List<Office>> GetTerminalsByCityAsync(string cityName, string? region, CancellationToken cancellationToken = default)
    {
        var query = _context.Offices
            .AsNoTracking()
            .Include(o => o.Phones)
            .Where(o => o.AddressCity != null && EF.Functions.ILike(o.AddressCity, $"%{cityName}%"));

        if (!string.IsNullOrWhiteSpace(region))
            query = query.Where(o => o.AddressRegion != null && EF.Functions.ILike(o.AddressRegion, $"%{region}%"));

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<int?> GetCityIdAsync(string cityName, string? region, CancellationToken cancellationToken = default)
    {
        var query = _context.Offices
            .AsNoTracking()
            .Where(o => o.AddressCity != null && EF.Functions.ILike(o.AddressCity, $"%{cityName}%"));

        if (!string.IsNullOrWhiteSpace(region))
            query = query.Where(o => o.AddressRegion != null && EF.Functions.ILike(o.AddressRegion, $"%{region}%"));

        var office = await query.FirstOrDefaultAsync(cancellationToken);
        return office?.CityCode;
    }
}
