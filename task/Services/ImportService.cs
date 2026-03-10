using Microsoft.EntityFrameworkCore;
using task.Contract;
using task.Data;
using task.Entities;

namespace task.Services;

internal sealed class ImportService(IServiceScopeFactory scopeFactory) : IImportService
{
    async Task IImportService.ImportAsync(IList<Office> offices, CancellationToken cancellationToken)
    {
        using (var scope = scopeFactory.CreateScope())
        {
            using (var dataContext = scope.ServiceProvider.GetRequiredService<DellinDictionaryDbContext>())
            {
                await dataContext.Offices.ExecuteDeleteAsync(cancellationToken);
                await dataContext.Offices.AddRangeAsync(offices, cancellationToken);
                await dataContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
