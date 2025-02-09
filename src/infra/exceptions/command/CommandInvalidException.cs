public class CommandInvalidException : CommandException
{
    public CommandInvalidException(string message) : base(message, StatusCodes.Status400BadRequest) { }
}