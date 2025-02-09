using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

public static class ExceptionHandlingExtensions
{
    public static IServiceCollection AddExceptionHandling(this IServiceCollection services)
    {
        services.Configure<ApiBehaviorOptions>(options =>
        {
            var errors = string.Empty;
            options.InvalidModelStateResponseFactory = context =>
            {
                errors = string.Join("; ", context.ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
                return new BadRequestObjectResult(new { error = errors });
            };
        });
        return services;
    }

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