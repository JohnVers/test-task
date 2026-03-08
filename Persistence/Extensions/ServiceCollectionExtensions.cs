using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Common;

namespace Persistence.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDbContext(this IServiceCollection serviceCollection, string? connectionString)
    {
        if (string.IsNullOrEmpty(connectionString))
            throw new ArgumentException("Строка подключения отсутствует.");
        
        return serviceCollection.AddDbContext<DellinDictionaryDbContext>(options =>
            options.UseNpgsql(connectionString));
    }
}