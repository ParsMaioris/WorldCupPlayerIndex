using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

public static class ServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();

        var swagger = configuration.GetSection("Swagger");
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc(swagger["Version"], new OpenApiInfo
            {
                Title = swagger["Title"],
                Version = swagger["Version"],
                Description = swagger["Description"]
            });
        });

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IPlayerRepository, EfPlayerRepository>();
        services.AddScoped<IPlayerDomainService, PlayerDomainService>();
        services.AddScoped<IPlayerApplicationService, PlayerApplicationService>();
        services.AddScoped<IPlayerCommandService, PlayerCommandService>();
        services.AddScoped<IPlayerQueryService, PlayerQueryService>();

        return services;
    }
}