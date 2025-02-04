using System.Collections.Concurrent;

public class FileLoggerProvider : ILoggerProvider
{
    private readonly FileLoggerOptions _options;
    private readonly ConcurrentDictionary<string, FileLogger> _loggers = new();

    public FileLoggerProvider(FileLoggerOptions options)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
        if (!Directory.Exists(_options.LogDirectory))
        {
            Directory.CreateDirectory(_options.LogDirectory);
        }
        CleanupOldLogs();
    }

    public ILogger CreateLogger(string categoryName)
    {
        if (string.IsNullOrWhiteSpace(categoryName))
            throw new ArgumentException("Category name cannot be null or whitespace.", nameof(categoryName));

        return _loggers.GetOrAdd(categoryName, name => new FileLogger(name, _options));
    }

    public void Dispose()
    {
    }

    private void CleanupOldLogs()
    {
        string searchPattern = $"{_options.FileNamePrefix}-*.log";
        string[] files = Directory.GetFiles(_options.LogDirectory, searchPattern);
        DateTime cutoffDate = DateTime.Now.AddDays(-_options.RetentionDays);

        foreach (var file in files)
        {
            string fileName = Path.GetFileNameWithoutExtension(file);
            int dashIndex = fileName.IndexOf('-');
            if (dashIndex < 0)
            {
                continue;
            }
            string datePart = fileName.Substring(dashIndex + 1);
            if (DateTime.TryParse(datePart, out DateTime logDate) && logDate < cutoffDate)
            {
                File.Delete(file);
            }
        }
    }
}