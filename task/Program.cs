using task.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithOptions();
builder.Services.AddSwagger();
builder.Services.AddApplicationServices(builder.Configuration);

var app = builder.Build();

app.UseSwaggerInDevelopment();
app.UseRouting();
app.MapControllers();
app.Run();
