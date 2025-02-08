using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

var fileLoggerOptions = builder.Configuration
    .GetSection("FileLoggerOptions")
    .Get<FileLoggerOptions>();

builder.Logging.ClearProviders();
if (fileLoggerOptions != null)
{
    builder.Logging.AddProvider(new FileLoggerProvider(fileLoggerOptions));
}
else
{
    throw new ArgumentNullException(nameof(fileLoggerOptions), "FileLoggerOptions cannot be null");
}
builder.Logging.AddConsole();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    var swaggerConfig = builder.Configuration.GetSection("Swagger");
    c.SwaggerDoc(swaggerConfig["Version"], new OpenApiInfo
    {
        Title = swaggerConfig["Title"],
        Version = swaggerConfig["Version"],
        Description = swaggerConfig["Description"]
    });
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IPlayerRepository, EfPlayerRepository>();
builder.Services.AddScoped<IPlayerDomainService, PlayerDomainService>();
builder.Services.AddScoped<IPlayerApplicationService, PlayerApplicationService>();
builder.Services.AddScoped<IPlayerCommandService, PlayerCommandService>();
builder.Services.AddScoped<IPlayerQueryService, PlayerQueryService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await dbContext.Database.EnsureCreatedAsync();

    if (!await dbContext.Players.AnyAsync())
    {
        dbContext.Players.AddRange(SeedData.CreateSamplePlayers());
        await dbContext.SaveChangesAsync();
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

await app.RunAsync();

public partial class Program { }