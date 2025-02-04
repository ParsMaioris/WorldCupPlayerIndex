using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.OpenApi.Models;

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
builder.Logging.AddConsole();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Player API",
        Version = "v1",
        Description = "An elegant API for managing players."
    });
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=app.db"));

builder.Services.AddScoped<IPlayerRepository, EfPlayerRepository>();
builder.Services.AddScoped<IPlayerDomainService, PlayerDomainService>();
builder.Services.AddScoped<IPlayerApplicationService, PlayerApplicationService>();
builder.Services.AddScoped<IPlayerCommandService, PlayerCommandService>();
builder.Services.AddScoped<IPlayerQueryService, PlayerQueryService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.EnsureCreated();

    if (!dbContext.Players.Any())
    {
        dbContext.Players.AddRange(SeedData.CreateSamplePlayers());
        dbContext.SaveChanges();
    }
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Player API V1");
});

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