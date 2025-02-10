using Serilog;
using Serilog.Events;
using Serilog.Sinks.SumoLogic;

public static class LoggingExtensions
{
    public static WebApplicationBuilder ConfigureLogging(this WebApplicationBuilder builder)
    {
        var config = builder.Configuration;

        var sumoLogicUrl = config.GetValue<string>("SumoLogic:Endpoint");
        if (string.IsNullOrWhiteSpace(sumoLogicUrl))
            throw new Exception("SumoLogic URL not configured under 'SumoLogic:SumoLogicUrl'.");

        var sourceName = config.GetValue<string>("SumoLogic:Source");
        if (string.IsNullOrWhiteSpace(sourceName))
            throw new Exception("Source name not configured under 'SumoLogic:Endpoint'.");

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .Enrich.FromLogContext()
            .WriteTo.SumoLogic(sumoLogicUrl, sourceName: sourceName, restrictedToMinimumLevel: LogEventLevel.Information)
            .CreateLogger();

        builder.Host.UseSerilog();
        return builder;
    }
}