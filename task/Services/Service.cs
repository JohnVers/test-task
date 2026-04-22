using Microsoft.EntityFrameworkCore;
using task.Contract;
using task.Data;
using task.Entities;

namespace task.Services
{
    internal sealed class Service(DellinDictionaryDbContext dataContext) : IService
    {
        #region ISevice Implementation

        /// <inheritdoc/>
        async Task<IList<Office>> IService.GetOfficesAsync(string cityName, string regionName, CancellationToken cancellationToken)
        {
            var result = await BuildOfficeLocationQuery(cityName, regionName).ToListAsync(cancellationToken);

            return result;
        }

        /// <inheritdoc/>
        async Task<int?> IService.GetCityIdAsync(string cityName, string regionName, CancellationToken cancellationToken)
        {
            var cityCode = await BuildOfficeLocationQuery(cityName, regionName)
                .Select(static o => o.CityCode)
                .FirstOrDefaultAsync(cancellationToken);

            return cityCode != 0 ? cityCode : null;
        }

        #endregion

        private IQueryable<Office> BuildOfficeLocationQuery(string cityName, string regionName)
        {
            var escapedCityName = EscapeLikePattern(cityName.Trim());

            var query = dataContext.Offices
                .AsNoTracking()
                .Where(o => o.AddressCity != null && EF.Functions.ILike(o.AddressCity, $"%{escapedCityName}%", @"\"));

            if (!string.IsNullOrWhiteSpace(regionName))
            {
                var escapedRegion = EscapeLikePattern(regionName.Trim());

                query = query.Where(o => o.AddressRegion != null && EF.Functions.ILike(o.AddressRegion, $"%{escapedRegion}%", @"\"));
            }

            return query;
        }

        private static string EscapeLikePattern(string value)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(value);

            return value
                .Replace(@"\", @"\\")
                .Replace("%", @"\%")
                .Replace("_", @"\_")
                .Replace("[", @"\[");
        }
    }
}
