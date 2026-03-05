using Microsoft.EntityFrameworkCore;
using task;
using task.Data;
using task.Map;

var builder = Host.CreateApplicationBuilder(args);
var services = builder.Services;

builder.Services.AddDbContextFactory<DellinDictionaryDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DellinDictionaryConnection")));

services.AddAutoMapper(typeof(MappingProfile));

builder.Services.Configure<WorkerOptions>(builder.Configuration.GetSection("WorkerOptions"));

services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
