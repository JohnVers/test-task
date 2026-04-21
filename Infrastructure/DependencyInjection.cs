using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using task.Application.Abstractions;
using task.Infrastructure.HostedServices;
using task.Infrastructure.Persistence;
using task.Infrastructure.Services;

namespace task.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<TerminalsDictionaryDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("Postgres")
                ?? throw new InvalidOperationException("ConnectionStrings: nulll");

            options.UseNpgsql(connectionString);
        });

        services.AddScoped<ITerminalImportService, TerminalImportService>();
        services.AddScoped<IOfficeQueryService, OfficeQueryService>();
        services.AddHostedService<TerminalImportBackgroundService>();

        return services;
    }
}
