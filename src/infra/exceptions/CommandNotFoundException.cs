public class CommandNotFoundException : CommandException
{
    public CommandNotFoundException(string message) : base(message, StatusCodes.Status404NotFound) { }
}