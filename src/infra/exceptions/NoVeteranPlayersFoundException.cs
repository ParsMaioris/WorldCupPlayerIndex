public class NoVeteranPlayersFoundException : DomainException
{
    public NoVeteranPlayersFoundException(string message = "No veteran players found.")
        : base(message)
    {
    }
}