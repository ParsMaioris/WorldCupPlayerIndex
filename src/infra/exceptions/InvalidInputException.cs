public class InvalidInputException : DomainException
{
    public InvalidInputException(string message) : base(message, StatusCodes.Status400BadRequest) { }
}