using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;

public static class EfCoreSqliteSetup
{
    public static AppDbContext CreateInMemoryContext()
    {
        var connection = new SqliteConnection("Data Source=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(connection)
            .Options;

        var context = new AppDbContext(options);

        context.Database.EnsureCreated();

        context.Players.AddRange(SeedData.CreateSamplePlayers());
        context.SaveChanges();

        return context;
    }
}