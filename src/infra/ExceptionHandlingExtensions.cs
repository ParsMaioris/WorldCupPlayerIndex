using Microsoft.AspNetCore.Diagnostics;

public static class ExceptionHandlingExtensions
{
    public static IApplicationBuilder ConfigureExceptionHandling(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(builder =>
        {
            builder.Run(async context =>
            {
                var error = context.Features.Get<IExceptionHandlerPathFeature>()?.Error;
                int statusCode = error is NotFoundException ? StatusCodes.Status404NotFound
                    : error is ForbiddenException ? StatusCodes.Status403Forbidden
                    : StatusCodes.Status500InternalServerError;
                context.Response.StatusCode = statusCode;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(new { error = error?.Message });
            });
        });
        return app;
    }
}