using task.Options;

namespace task.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ConfigureOptions(this IServiceCollection serviceCollection, ConfigurationManager configurationManager)
    {
        serviceCollection.Configure<FileSettings>(configurationManager.GetSection(FileSettings.Section));
        serviceCollection.Configure<JobSettings>(configurationManager.GetSection(JobSettings.Section));
        
        return serviceCollection;
    }
}