public class QueryInvalidException : QueryException
{
    public QueryInvalidException(string message)
        : base(message, StatusCodes.Status400BadRequest) { }
}