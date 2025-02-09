using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class ApiExceptionFilter : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        context.Result = CreateResultForException(context.Exception);
        context.ExceptionHandled = true;
    }

    private ObjectResult CreateResultForException(Exception exception)
    {
        if (IsBadRequestException(exception))
        {
            return new BadRequestObjectResult(new { error = exception.Message });
        }
        if (IsNotFoundException(exception))
        {
            return new NotFoundObjectResult(new { error = exception.Message });
        }
        if (exception is CommandForbiddenException)
        {
            return new ObjectResult(new { error = exception.Message }) { StatusCode = 403 };
        }
        return new ObjectResult(new { error = "An unexpected error occurred." }) { StatusCode = 500 };
    }

    private bool IsBadRequestException(Exception exception)
    {
        return exception is CommandInvalidException || exception is QueryInvalidException;
    }

    private bool IsNotFoundException(Exception exception)
    {
        return exception is CommandNotFoundException ||
               exception is QueryNotFoundException ||
               exception is NoVeteranPlayersFoundException ||
               exception is NoPlayersForPerformanceRankingException;
    }
}