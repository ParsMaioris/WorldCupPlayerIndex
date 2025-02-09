public class CommandForbiddenException : CommandException
{
    public CommandForbiddenException(string message)
        : base(message, StatusCodes.Status403Forbidden) { }
}