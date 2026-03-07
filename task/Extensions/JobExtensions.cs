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
                    .WithCronSchedule(jobSettings.ImportTerminalsSchedule));
        });

        serviceCollection.AddQuartzHostedService(options =>
        {
            options.WaitForJobsToComplete = true;
            options.AwaitApplicationStarted = true;
        });

        return serviceCollection;
    }
}