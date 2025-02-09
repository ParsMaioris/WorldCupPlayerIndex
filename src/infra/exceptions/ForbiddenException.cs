public class ForbiddenException : CommandException
{
    public ForbiddenException(string message) : base(message, StatusCodes.Status403Forbidden) { }
}