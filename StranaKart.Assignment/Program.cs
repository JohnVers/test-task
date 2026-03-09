using Microsoft.EntityFrameworkCore;
using Serilog;
using StranaKart.Assignment;
using StranaKart.Assignment.Infrastructure;

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
    .CreateBootstrapLogger();

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSerilog((services, configuration) =>
{
    configuration
        .ReadFrom.Configuration(builder.Configuration)
        .ReadFrom.Services(services);
});

builder.Services.AddScheduling(builder.Configuration);
builder.Services.AddDatabaseServices(builder.Configuration);
builder.Services.AddApplicationServices();

builder.Services.AddHostedService<TerminalImportWorker>();

var host = builder.Build();

using (var scope = host.Services.CreateScope())
{
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    var db = scope.ServiceProvider.GetRequiredService<DellinDictionaryDbContext>();

    try
    {
        logger.LogInformation("Применение миграций БД...");
        await db.Database.MigrateAsync();
        logger.LogInformation("Миграции применены");
    }
    catch (Exception ex)
    {
        logger.LogCritical(ex, "Не удалось применить миграции. Приложение остановлено");
        throw;
    }
}

host.Run();