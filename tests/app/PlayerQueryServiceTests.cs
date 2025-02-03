using Microsoft.Extensions.DependencyInjection;

[TestClass]
public class PlayerQueryServiceTests
{
    [DataTestMethod]
    [DataRow(TestMode.Unit)]
    [DataRow(TestMode.Integration)]
    public void Test_GetPlayersOlderThan(TestMode mode)
    {
        var provider = TestSetupFactory.CreateServiceProvider(mode);
        var queryService = provider.GetRequiredService<IPlayerQueryService>();

        var players = queryService.GetPlayersOlderThan(30).ToList();

        Assert.IsTrue(players.Any(p => p.Name == "Cristiano Ronaldo"));
        Assert.IsTrue(players.Any(p => p.Name == "Lionel Messi"));
        Assert.IsTrue(players.Any(p => p.Name == "Robert Lewandowski"));
        Assert.IsFalse(players.Any(p => p.Name == "Neymar Jr"));
    }

    [DataTestMethod]
    [DataRow(TestMode.Unit)]
    [DataRow(TestMode.Integration)]
    public void Test_GetPlayersByNationality(TestMode mode)
    {
        var provider = TestSetupFactory.CreateServiceProvider(mode);
        var queryService = provider.GetRequiredService<IPlayerQueryService>();

        var portuguesePlayers = queryService.GetPlayersByNationality("Portugal");

        Assert.IsTrue(portuguesePlayers.Any(p => p.Name == "Cristiano Ronaldo"), "Expected to find Cristiano Ronaldo.");
        Assert.IsTrue(portuguesePlayers.Any(p => p.Name == "Bruno Fernandes"), "Expected to find Bruno Fernandes.");

        Assert.IsTrue(portuguesePlayers.All(p => p.Nationality.Equals("Portugal", StringComparison.OrdinalIgnoreCase)),
            "Every player must be from Portugal.");
    }
}