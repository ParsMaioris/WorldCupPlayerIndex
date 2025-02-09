using Microsoft.AspNetCore.Diagnostics;

public static class ExceptionHandlingExtensions
{
    public static IApplicationBuilder ConfigureExceptionHandling(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(builder =>
        {
            builder.Run(async context =>
            {
                var exception = context.Features.Get<IExceptionHandlerPathFeature>()?.Error;

                int statusCode = exception switch
                {
                    JwtAuthenticationException => StatusCodes.Status403Forbidden,
                    PersistenceException => StatusCodes.Status500InternalServerError,
                    _ => StatusCodes.Status500InternalServerError
                };

                context.Response.StatusCode = statusCode;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(new { error = exception?.Message });
            });
        });

        return app;
    }
}