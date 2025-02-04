public class FileLogger : ILogger
{
    private readonly string _categoryName;
    private readonly FileLoggerOptions _options;
    private readonly object _lock = new();

    public FileLogger(string categoryName, FileLoggerOptions options)
    {
        _categoryName = categoryName ?? throw new ArgumentNullException(nameof(categoryName));
        _options = options ?? throw new ArgumentNullException(nameof(options));
    }

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

    public bool IsEnabled(LogLevel logLevel) => logLevel >= _options.MinimumLogLevel;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel))
        {
            return;
        }

        if (formatter == null)
        {
            throw new ArgumentNullException(nameof(formatter));
        }

        string message = formatter(state, exception);
        if (string.IsNullOrWhiteSpace(message))
        {
            return;
        }

        string logRecord = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} [{logLevel}] {_categoryName}: {message}";
        if (exception is not null)
        {
            logRecord += Environment.NewLine + exception;
        }

        string fileName = $"{_options.FileNamePrefix}-{DateTime.Now:yyyy-MM-dd}.log";
        string filePath = Path.Combine(_options.LogDirectory, fileName);

        lock (_lock)
        {
            File.AppendAllText(filePath, logRecord + Environment.NewLine);
        }
    }
}