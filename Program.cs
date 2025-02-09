var builder = WebApplication.CreateBuilder(args);

builder.ConfigureLogging();
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddExceptionHandling();

var app = builder.Build();

app.ConfigureExceptionHandling();
app.ConfigureMiddleware();

app.Run();

public partial class Program { }