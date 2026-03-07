using Persistence.Extensions;
using task;
using task.Extensions;

var builder = Host.CreateApplicationBuilder(args);

// Регистрация DbContext
builder.Services.AddDbContext(builder.Configuration.GetConnectionString("DefaultConnection"));

// Регистрация сервисов
builder.Services.AddHostedService<Worker>();

// Конфигурация настроек
builder.Services.ConfigureOptions(builder.Configuration);

var host = builder.Build();
host.Run();
