[TestClass]
public class PlayerCommandServiceTests
{
    [TestMethod]
    public async Task Test_RecordGoal_Unit()
    {
        var provider = TestSetupFactory.CreateServiceProvider(TestMode.Unit);
        var repository = provider.GetRequiredService<IPlayerRepository>();
        var commandService = provider.GetRequiredService<IPlayerCommandService>();

        var allPlayers = await repository.GetAllPlayersAsync();
        var original = allPlayers.First(p => p.Name == "Lionel Messi");

        var updated = await commandService.RecordGoalAsync("Lionel Messi");

        Assert.AreEqual(original.GoalsScored + 1, updated.GoalsScored);
    }

    [TestMethod]
    public async Task Test_RecordGoal_Integration()
    {
        var provider = TestSetupFactory.CreateServiceProvider(TestMode.Integration);
        var repository = provider.GetRequiredService<IPlayerRepository>();
        var commandService = provider.GetRequiredService<IPlayerCommandService>();

        var allPlayers = await repository.GetAllPlayersAsync();
        var messi = allPlayers.First(p => p.Name == "Lionel Messi");
        int initialGoalCount = messi.GoalsScored;

        await commandService.RecordGoalAsync("Lionel Messi");

        var updatedPlayers = await repository.GetAllPlayersAsync();
        var updated = updatedPlayers.First(p => p.Name == "Lionel Messi");

        Assert.AreEqual(initialGoalCount + 1, updated.GoalsScored);
    }

    [TestMethod]
    public async Task Test_RecordGoals_Unit()
    {
        var provider = TestSetupFactory.CreateServiceProvider(TestMode.Unit);
        var repository = provider.GetRequiredService<IPlayerRepository>();
        var commandService = provider.GetRequiredService<IPlayerCommandService>();

        var playerNames = new List<string> { "Lionel Messi", "Cristiano Ronaldo" };
        var initialPlayers = (await repository.GetAllPlayersAsync())
            .Where(p => playerNames.Contains(p.Name))
            .ToDictionary(p => p.Name, p => p.GoalsScored);

        var updatedPlayers = (await commandService.RecordGoalsAsync(playerNames)).ToList();

        foreach (var player in updatedPlayers)
        {
            Assert.AreEqual(initialPlayers[player.Name] + 1, player.GoalsScored);
        }
    }

    [TestMethod]
    public async Task Test_RecordGoals_Integration()
    {
        var provider = TestSetupFactory.CreateServiceProvider(TestMode.Integration);
        var repository = provider.GetRequiredService<IPlayerRepository>();
        var commandService = provider.GetRequiredService<IPlayerCommandService>();

        var playerNames = new List<string> { "Lionel Messi", "Cristiano Ronaldo" };
        var initialPlayers = (await repository.GetAllPlayersAsync())
            .Where(p => playerNames.Contains(p.Name))
            .ToDictionary(p => p.Name, p => p.GoalsScored);

        await commandService.RecordGoalsAsync(playerNames);

        var updatedPlayers = (await repository.GetAllPlayersAsync())
            .Where(p => playerNames.Contains(p.Name))
            .ToDictionary(p => p.Name, p => p.GoalsScored);

        foreach (var name in playerNames)
        {
            Assert.AreEqual(initialPlayers[name] + 1, updatedPlayers[name]);
        }
    }
}