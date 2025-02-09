public abstract class AuthenticationException : Exception
{
    public int StatusCode { get; }

    protected AuthenticationException(string message, int statusCode = StatusCodes.Status403Forbidden)
        : base(message)
    {
        StatusCode = statusCode;
    }
}