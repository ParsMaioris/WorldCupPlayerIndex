public class QueryNotFoundException : QueryException
{
    public QueryNotFoundException(string message)
        : base(message, StatusCodes.Status404NotFound) { }
}