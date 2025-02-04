public class FileLoggerOptions
{
    public string LogDirectory { get; set; } = "Logs";
    public string FileNamePrefix { get; set; } = "app";
    public int RetentionDays { get; set; } = 7;
    public LogLevel MinimumLogLevel { get; set; } = LogLevel.Warning;
}