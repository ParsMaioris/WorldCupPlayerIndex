public abstract class QueryException : Exception
{
    public int StatusCode { get; }

    protected QueryException(string message, int statusCode) : base(message)
    {
        StatusCode = statusCode;
    }
}