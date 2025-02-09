using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly Func<IEnumerable<Player>, IEnumerable<Player>>? _seedPlayersFilter;

    public CustomWebApplicationFactory() : this(null) { }

    public CustomWebApplicationFactory(Func<IEnumerable<Player>, IEnumerable<Player>>? seedPlayersFilter)
    {
        _seedPlayersFilter = seedPlayersFilter;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var projectDir = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "../../../"));
        builder.UseContentRoot(projectDir);
        builder.UseEnvironment("Test");

        builder.ConfigureAppConfiguration((context, config) =>
        {
            config.SetBasePath(projectDir)
                  .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        });

        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            var connection = new SqliteConnection("Data Source=:memory:");
            connection.Open();

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlite(connection);
            });

            var sp = services.BuildServiceProvider();

            using (var scope = sp.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<AppDbContext>();
                var logger = scopedServices.GetRequiredService<ILogger<CustomWebApplicationFactory>>();

                try
                {
                    db.Database.EnsureCreatedAsync().GetAwaiter().GetResult();

                    if (!db.Players.Any())
                    {
                        var players = SeedData.CreateSamplePlayers();
                        if (_seedPlayersFilter != null)
                        {
                            players = _seedPlayersFilter(players);
                        }
                        db.Players.AddRange(players);
                        db.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred creating and seeding the test database.");
                    throw;
                }
            }
        });

        base.ConfigureWebHost(builder);
    }
}