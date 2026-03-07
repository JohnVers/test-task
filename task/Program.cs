using Application.Extensions;
using Persistence.Extensions;
using task.Extensions;

var builder = Host.CreateApplicationBuilder(args);

// Конфигурация настроек
builder.Services.ConfigureOptions(builder.Configuration);

// Регистрация DbContext
builder.Services.AddDbContext(builder.Configuration.GetConnectionString("DefaultConnection"));

// Регистрация слоя приложения
builder.Services.AddApplication();

builder.Services.ConfigureJobs();

var host = builder.Build();
host.Run();
