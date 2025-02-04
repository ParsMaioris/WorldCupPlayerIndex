using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

var fileLoggerOptions = new FileLoggerOptions
{
    LogDirectory = "Logs",
    FileNamePrefix = "app",
    RetentionDays = 7,
    MinimumLogLevel = LogLevel.Warning
};

builder.Logging.ClearProviders();
builder.Logging.AddProvider(new FileLoggerProvider(fileLoggerOptions));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=app.db"));

builder.Services.AddScoped<IPlayerRepository, EfPlayerRepository>();
builder.Services.AddScoped<IPlayerDomainService, PlayerDomainService>();
builder.Services.AddScoped<IPlayerApplicationService, PlayerApplicationService>();
builder.Services.AddScoped<IPlayerCommandService, PlayerCommandService>();
builder.Services.AddScoped<IPlayerQueryService, PlayerQueryService>();

var app = builder.Build();

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerPathFeature>();
        var error = exceptionHandlerFeature?.Error;

        int statusCode = error switch
        {
            NotFoundException => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError
        };

        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";
        var response = new { error = error?.Message };
        await context.Response.WriteAsJsonAsync(response);
    });
});

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }