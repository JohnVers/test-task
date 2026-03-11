using task.Contract;
using task.Services;

namespace task.DependecyInjection;

public static class ServiceCollectionExtensions
{
    public static void AddInfrastructureServices(this IServiceCollection seviceCollection)
    {
        seviceCollection.AddSingleton<IImportService, ImportService>();
        seviceCollection.AddSingleton<IDataSourceService, DataSourceService>();
    }
}
