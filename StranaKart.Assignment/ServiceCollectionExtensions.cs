using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Npgsql;
using StranaKart.Assignment.Domain.Entities;
using StranaKart.Assignment.Infrastructure;
using StranaKart.Assignment.Infrastructure.Scheduling;
using StranaKart.Assignment.Infrastructure.Scheduling.Options;
using StranaKart.Assignment.Services;
using StranaKart.Assignment.Services.Options;

namespace StranaKart.Assignment;

public static class ServiceCollectionExtensions
{
    private const string ConnectionStringName = "DefaultConnection";

    public static IServiceCollection AddScheduling(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<SimpleScheduleOptions>()
            .Bind(configuration.GetSection(SimpleScheduleOptions.SectionName))
            .ValidateOnStart();

        services.AddSingleton<IValidateOptions<SimpleScheduleOptions>, SimpleScheduleOptionsValidator>();

        services.AddSingleton<IScheduleService, SimpleScheduleService>();

        return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddOptions<SyncOptions>()
            .BindConfiguration(SyncOptions.SectionName)
            .ValidateOnStart();

        services.AddSingleton<IValidateOptions<SyncOptions>, SyncOptionsValidator>();

        services.AddScoped<ITerminalProviderService, JsonFileTerminalProviderService>();
        services.AddScoped<ITerminalSynchronizationService, TerminalSynchronizationService>();

        services.AddSingleton(TimeProvider.System);

        return services;
    }

    public static IServiceCollection AddDatabaseServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(sp =>
        {
            var dataSource = new NpgsqlDataSourceBuilder(configuration.GetConnectionString(ConnectionStringName))
                .MapEnum<OfficeType>()
                .Build();

            return dataSource;
        });

        services.AddDbContext<DellinDictionaryDbContext>((sp, options) =>
        {
            var dataSource = sp.GetRequiredService<NpgsqlDataSource>();

            options.UseNpgsql(dataSource);
            options.UseSnakeCaseNamingConvention();
        });

        return services;
    }
}
