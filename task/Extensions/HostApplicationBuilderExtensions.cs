using Serilog;

namespace task.Extensions;

public static class HostApplicationBuilderExtensions
{
    public static HostApplicationBuilder ConfigureLog(this HostApplicationBuilder builder)
    {
        builder.Logging.ClearProviders();
        Log.Logger = new LoggerConfiguration()
            .ReadFrom
            .Configuration(builder.Configuration)
            .CreateLogger();

        builder.Logging.AddSerilog(Log.Logger);

        return builder;
    }
}