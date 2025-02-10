using Serilog;
using Serilog.Events;
using Serilog.Sinks.SumoLogic;

public static class LoggingExtensions
{
    public static WebApplicationBuilder ConfigureLogging(this WebApplicationBuilder builder)
    {
        var config = builder.Configuration;

        var sumoLogicUrl = config.GetValue<string>("CustomLogging:SumoLogicUrl");
        if (string.IsNullOrWhiteSpace(sumoLogicUrl))
            throw new Exception("SumoLogic URL not configured under 'CustomLogging:SumoLogicUrl'.");

        var sourceName = config.GetValue<string>("CustomLogging:SourceName");
        if (string.IsNullOrWhiteSpace(sourceName))
            throw new Exception("Source name not configured under 'CustomLogging:SourceName'.");

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .Enrich.FromLogContext()
            .WriteTo.SumoLogic(sumoLogicUrl, sourceName: sourceName, restrictedToMinimumLevel: LogEventLevel.Information)
            .CreateLogger();

        builder.Host.UseSerilog();
        return builder;
    }
}