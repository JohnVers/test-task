using Microsoft.Extensions.Options;
using Quartz;
using task.Jobs;
using task.Options;

namespace task.Extensions;

public static class JobExtensions
{
    public static IServiceCollection ConfigureJobs(this IServiceCollection serviceCollection)
    {
        var jobSettings = serviceCollection.BuildServiceProvider().GetRequiredService<IOptions<JobSettings>>().Value;

        if (string.IsNullOrEmpty(jobSettings.ImportTerminalsSchedule))
            throw new ArgumentException("Расписание для импорт терминалов отсутствует.");
        
        serviceCollection.AddQuartz(configurator =>
        {
            var jobKey = new JobKey("ImportTerminalsJob");
            
            configurator.AddJob<ImportTerminalsJob>(jobConfigurator => jobConfigurator.WithIdentity(jobKey));

            configurator.AddTrigger(triggerConfigurator =>
                triggerConfigurator
                    .ForJob(jobKey)
                    .WithIdentity("ImportTerminalsTrigger")
                    .WithCronSchedule(jobSettings.ImportTerminalsSchedule, builder =>
                    {
                        builder.InTimeZone(GetMoscowTimeZone());
                    }));
        });

        serviceCollection.AddQuartzHostedService(options =>
        {
            options.WaitForJobsToComplete = true;
            options.AwaitApplicationStarted = true;
        });

        return serviceCollection;
    }

    private static TimeZoneInfo GetMoscowTimeZone()
    {
        TimeZoneInfo moscowTimeZone;
        try
        {
            // Пробуем Linux-идентификатор (также работает на macOS)
            moscowTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Moscow");
        }
        catch (TimeZoneNotFoundException)
        {
            // Если не найден, пробуем Windows-идентификатор
            moscowTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time");
        }

        return moscowTimeZone;
    }
}