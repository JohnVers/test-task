using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using task.Application.DTOs;
using task.Application.Services.Interfaces;
using test.Core;
using test.Core.Entities;
using test.Core.Repositories;

namespace task.Application.Services.Implementation
{
    public class OfficeService : IOfficeService
    {
        private readonly DellinDictionaryDbContext _dbContext;
        private readonly OfficeRepository _repository;

        public OfficeService(DellinDictionaryDbContext dbContext)
        {
            _dbContext = dbContext;

            _repository = new OfficeRepository(_dbContext);
        }

        public async Task<WorkStatsDTO> SaveData(IList<Office> offices, CancellationToken cancellationToken)
        {
            int totalItemsCount = offices.Count;

            var dbOffices = await _repository
                                    .FindAll()
                                    .ConfigureAwait(false);

            var dbOfficeDictionary = dbOffices.ToDictionary(o => o.Uuid);

            var officeDictionary = offices.ToDictionary(o => o.Uuid);

            var officesToUpdate = offices.Select(o => o.Uuid).Intersect(dbOffices.Select(o=>o.Uuid));

            var officesToDelete = dbOffices.Select(o => o.Uuid).Except(offices.Select(o => o.Uuid));

            var officesToAdd = offices.Select(o => o.Uuid).Except(dbOffices.Select(o => o.Uuid));

            int totalDeleted = officesToDelete.Count();
            int totalSaved = officesToAdd.Count();

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try 
                {
                    await DeleteOffices(dbOfficeDictionary, officesToDelete, cancellationToken)
                    .ConfigureAwait(false);

                    await AddOffices(officeDictionary, officesToAdd, cancellationToken)
                        .ConfigureAwait(false);

                    await UpdateOffices(dbOfficeDictionary, officeDictionary, officesToUpdate, cancellationToken)
                        .ConfigureAwait(false);

                    transaction.Commit();

                    return new WorkStatsDTO(totalItemsCount, totalDeleted, totalSaved, null);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    return new WorkStatsDTO(0, 0, 0, ex.InnerException?.Message ?? ex.Message);
                }
            }

        }

        private async Task DeleteOffices(Dictionary<string, Office> officeDictionary, IEnumerable<string> names, CancellationToken cancellationToken)
        {
            foreach (var name in names)
            {
                if (!officeDictionary.TryGetValue(name, out var office))
                    throw new Exception($"Delete office error: {name}");

                await _repository.DeleteOffice(office, cancellationToken).ConfigureAwait(false);
            }
        }

        private async Task AddOffices(Dictionary<string, Office> officeDictionary, IEnumerable<string> names, CancellationToken cancellationToken)
        {
            foreach (var name in names)
            {
                if (!officeDictionary.TryGetValue(name, out var office))
                    throw new Exception($"Add office error: {name}");

                await _repository.AddOffice(office, cancellationToken).ConfigureAwait(false);
            }
        }

        private async Task UpdateOffices(Dictionary<string, Office> dbOfficeDictionary, Dictionary<string, Office> officeDictionary, IEnumerable<string> names, CancellationToken cancellationToken)
        {
            foreach (var name in names)
            {
                if (!officeDictionary.TryGetValue(name, out var office))
                    throw new Exception($"Update office error: {name}");

                if (!dbOfficeDictionary.TryGetValue(name, out var dbOffice))
                    throw new Exception($"Update office error: {name}");

                MapOffice(office, dbOffice);

                await _repository.UpdateOffice(dbOffice, cancellationToken).ConfigureAwait(false);
            }
        }

        private void MapOffice(Office src, Office dst)
        {
            dst.Address = src.Address;
            dst.WorkTime = src.WorkTime;
            dst.CityCode = src.CityCode;
            dst.Code = src.Code;
            dst.Phones = src.Phones;
            dst.WorkTime = src.WorkTime;
            dst.Coordinate = src.Coordinate;
            dst.Type = dst.Type;
        }
    }
}
