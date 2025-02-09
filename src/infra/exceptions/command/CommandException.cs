public abstract class CommandException : Exception
{
    public int StatusCode { get; }

    protected CommandException(string message, int statusCode) : base(message)
    {
        StatusCode = statusCode;
    }
}