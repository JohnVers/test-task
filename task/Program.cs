using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Console;
using task;
using task.Data;
using task.Interfaces;
using task.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole(options =>
{
	options.FormatterName = ConsoleFormatterNames.Simple;
});

builder.Logging.AddSimpleConsole(options =>
{
	options.TimestampFormat = "[yyyy-MM-dd HH:mm:ss] ";
	options.IncludeScopes = false;
	options.SingleLine = true;
});

builder.Services.AddDbContext<DellinDictionaryDbContext>(options =>
{
	options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
	options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});

builder.Services.AddScoped<IImportService, ImportService>();

builder.Services.AddHostedService<Worker>();

builder.Services.AddControllers().AddJsonOptions(options =>
{
	options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
	options.JsonSerializerOptions.WriteIndented = true; 
}); 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseRouting();
app.MapControllers(); // No authorization needed
app.Run();