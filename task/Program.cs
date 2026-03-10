using Microsoft.EntityFrameworkCore;
using task;
using task.Application.Services.Implementation;
using task.Application.Services.Interfaces;
using test.Core;

var builder = Host.CreateApplicationBuilder(args);

ConfigureServices(builder.Services);

builder.Services.AddHostedService<Worker>();




var host = builder.Build();
host.Run();


void ConfigureServices(IServiceCollection services)
{
    var configurationBuilder = new ConfigurationBuilder().AddJsonFile("appsettings.json");

#if DEBUG
    configurationBuilder.AddJsonFile("appsettings.Development.json");
#endif

    services.AddLogging(configure =>
    {
        configure.AddConsole();
        configure.AddDebug();
    });

    var configuration = configurationBuilder.Build();

    services.AddSingleton<IConfigurationRoot>(_ => configuration);

    var connectionString = configuration.GetConnectionString("DefaultConnection");

    services.AddDbContext<DellinDictionaryDbContext>(options => options.UseNpgsql(connectionString), ServiceLifetime.Singleton);

    services.AddSingleton<IConfigurationService, ConfigurationService>();

    services.AddSingleton<IParsingService, ParsingService>();

    services.AddSingleton<IMappingService, MappingService>();

    services.AddSingleton<IOfficeService, OfficeService>();
}