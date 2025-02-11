using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;

public static class EfCoreSqliteSetup
{
    public static AppDbContext CreateInMemoryContext()
    {
        try
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
        catch (Exception ex)
        {
            throw new PersistenceException("Failed to create the in-memory SQLite context.", ex);
        }
    }
}