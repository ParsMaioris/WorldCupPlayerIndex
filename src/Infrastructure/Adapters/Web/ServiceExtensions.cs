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
            var swagger = configuration.GetSection("Swagger");
            c.SwaggerDoc(swagger["Version"], new OpenApiInfo
            {
                Title = swagger["Title"],
                Version = swagger["Version"],
                Description = swagger["Description"]
            });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "Enter 'Bearer' [space] and then your token",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme {
                        Reference = new OpenApiReference {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
            });
        });

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IPlayerRepository, EfPlayerRepository>();
        services.AddScoped<IPlayerDomainService, PlayerDomainService>();
        services.AddScoped<IPlayerApplicationService, PlayerApplicationService>();
        services.AddScoped<IPlayerCommandService, PlayerCommandService>();
        services.AddScoped<IPlayerQueryService, PlayerQueryService>();

        services.AddJwtAuth(configuration);
        services.AddHttpContextAccessor();
        services.AddScoped<IJwtTokenService, LocalNetworkJwtTokenService>();

        return services;
    }
}