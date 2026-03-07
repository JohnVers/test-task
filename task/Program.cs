using Persistence.Extensions;
using task;
using task.Extensions;

var builder = Host.CreateApplicationBuilder(args);

// Конфигурация настроек
builder.Services.ConfigureOptions(builder.Configuration);

// Регистрация DbContext
builder.Services.AddDbContext(builder.Configuration.GetConnectionString("DefaultConnection"));

builder.Services.ConfigureJobs();

var host = builder.Build();
host.Run();
