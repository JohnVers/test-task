using Application.Extensions;
using Persistence.Extensions;
using task.Extensions;

var builder = Host.CreateApplicationBuilder(args)
    .ConfigureLog();

builder.Services
    .ConfigureOptions(builder.Configuration)
    .AddDbContext(builder.Configuration.GetConnectionString("DefaultConnection"))
    .AddApplicationLayer()
    .ConfigureJobs();

var host = builder.Build();
host.Run();