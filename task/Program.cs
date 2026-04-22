using Application;
using Application.UseCases.Queries.GetCityId;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Diagnostics;
using System.Reflection;

public class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Добавляем контроллеры
        builder.Services.AddControllers();

        // Добавляем Swagger
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "TestTask API",
                Version = "v1",
                Description = "API для работы со справочником терминалов"
            });

            // XML комментарии (опционально)
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            if (File.Exists(xmlPath))
                options.IncludeXmlComments(xmlPath);
        });

        // База данных PostgreSQL
        builder.Services.AddDbContext<DellinDictionaryDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

        // Фоновая служба для импорта
        builder.Services.AddHostedService<TerminalImportBackgroundService>();

        // MediatR
        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(GetCityIdHandler).Assembly);
        });

        var app = builder.Build();

        // Настройка Swagger
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "TestTask API V1");
                c.RoutePrefix = "swagger";

                try
                {
                    var launchUrl = "https://localhost:5001/swagger"; // Укажите ваш порт
                    var browserPath = Environment.GetEnvironmentVariable("BROWSER_PATH");

                    if (string.IsNullOrEmpty(browserPath))
                        Process.Start(new ProcessStartInfo(launchUrl) { UseShellExecute = true });
                    else
                        Process.Start(browserPath, launchUrl);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Не удалось открыть браузер: {ex.Message}");
                }
            });
        }

        // Создание базы данных
        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<DellinDictionaryDbContext>();
            await db.Database.EnsureCreatedAsync();
        }

        app.UseRouting();
        app.MapControllers();
        app.Run();
    }
}