using Microsoft.EntityFrameworkCore;
using task;
using task.Options;
using task.Persistence.Common;

var builder = Host.CreateApplicationBuilder(args);

// Регистрация DbContext
builder.Services.AddDbContext<DellinDictionaryDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Регистрация сервисов
builder.Services.AddHostedService<Worker>();

// Конфигурация пути к файлу
builder.Services.Configure<FileSettings>(builder.Configuration.GetSection("FileSettings"));

var host = builder.Build();
host.Run();
