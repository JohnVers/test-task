using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using test.Core.Entities;

namespace test.Core.Repositories
{
    public class OfficeRepository
    {
        private readonly DellinDictionaryDbContext _dbContext;

        public OfficeRepository(DellinDictionaryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IList<Office>> FindAll()
        {
            return await _dbContext.Offices
                .Include(o => o.Coordinate)
                .Include(o => o.Phones)
                .ToListAsync()
                .ConfigureAwait(false);
        }

        public async Task DeleteOffice(Office office, CancellationToken cancellationToken)
        {
            _dbContext.Offices
                .Remove(office);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteOffices(IList<Office> offices, CancellationToken cancellationToken)
        {
            _dbContext.Offices
                .RemoveRange(offices);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task AddOffice(Office office, CancellationToken cancellationToken)
        {
            await _dbContext.Offices
                .AddAsync(office, cancellationToken)
                .ConfigureAwait(false);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task AddOffices(IList<Office> offices, CancellationToken cancellationToken)
        {
            await _dbContext.Offices
                .AddRangeAsync(offices)
                .ConfigureAwait(false);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateOffice(Office office, CancellationToken cancellationToken)
        {
            _dbContext.Offices
                .Update(office);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateOffices(IList<Office> offices, CancellationToken cancellationToken)
        {
            _dbContext.Offices
                .UpdateRange(offices);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
