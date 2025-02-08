public static class LoggingExtensions
{
    public static void ConfigureLogging(this WebApplicationBuilder builder)
    {
        var fileLoggerOptions = builder.Configuration
            .GetSection("FileLoggerOptions")
            .Get<FileLoggerOptions>()
            ?? throw new ArgumentNullException("FileLoggerOptions cannot be null");

        builder.Logging.ClearProviders();
        builder.Logging.AddProvider(new FileLoggerProvider(fileLoggerOptions));
        builder.Logging.AddConsole();
    }
}