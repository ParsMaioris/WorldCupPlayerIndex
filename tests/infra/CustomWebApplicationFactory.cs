using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var projectDir = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "../../../src/infra"));
        builder.UseContentRoot(projectDir);
        builder.UseEnvironment("Development");

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
                    db.Database.EnsureCreated();

                    if (!db.Players.Any())
                    {
                        db.Players.AddRange(SeedData.CreateSamplePlayers());
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