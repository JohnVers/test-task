using Microsoft.EntityFrameworkCore;
using task.Data;
using task.Services;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddDbContext<DellinDictionaryDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseNpgsql(connectionString);
});

builder.Services.AddHostedService<TerminalsImportService>();

var host = builder.Build();

await host.RunAsync();
