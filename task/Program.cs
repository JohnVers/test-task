using Microsoft.EntityFrameworkCore;
using task;
using task.Data;
using task.DependecyInjection;
using task.Options;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddOptions();
builder.Services.AddDbContext<DellinDictionaryDbContext>(options=>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default"));
});
builder.Services.Configure<ImportOptions>(builder.Configuration.GetSection(nameof(ImportOptions)));
builder.Services.AddHostedService<Worker>();
builder.Services.AddInfrastructureServices();

var host = builder.Build();

using (var scope = host.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var dbContext = services.GetRequiredService<DellinDictionaryDbContext>();
        dbContext.Database.EnsureCreated();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Ошибка создания БД");
    }
}

host.Run();
