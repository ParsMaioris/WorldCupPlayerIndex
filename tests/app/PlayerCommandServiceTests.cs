using Microsoft.Extensions.DependencyInjection;

[TestClass]
public class PlayerCommandServiceTests
{
    [TestMethod]
    public void Test_RecordGoal_Unit()
    {
        var provider = TestSetupFactory.CreateServiceProvider(TestMode.Unit);

        var repository = provider.GetRequiredService<IPlayerRepository>();
        var commandService = provider.GetRequiredService<IPlayerCommandService>();

        var original = repository.GetAllPlayers().First(p => p.Name == "Lionel Messi");

        var updated = commandService.RecordGoal("Lionel Messi");

        Assert.AreEqual(original.GoalsScored + 1, updated.GoalsScored);
    }

    [TestMethod]
    public void Test_RecordGoal_Integration()
    {
        var provider = TestSetupFactory.CreateServiceProvider(TestMode.Integration);

        var repository = provider.GetRequiredService<IPlayerRepository>();
        var commandService = provider.GetRequiredService<IPlayerCommandService>();

        var messi = repository.GetAllPlayers().First(p => p.Name == "Lionel Messi");
        int initialGoalCount = messi.GoalsScored;

        commandService.RecordGoal("Lionel Messi");

        var updated = repository.GetAllPlayers().First(p => p.Name == "Lionel Messi");

        Assert.AreEqual(initialGoalCount + 1, updated.GoalsScored);
    }

    [TestMethod]
    public void Test_RecordGoals_Unit()
    {
        var provider = TestSetupFactory.CreateServiceProvider(TestMode.Unit);

        var repository = provider.GetRequiredService<IPlayerRepository>();
        var commandService = provider.GetRequiredService<IPlayerCommandService>();

        var playerNames = new List<string> { "Lionel Messi", "Cristiano Ronaldo" };

        var initialPlayers = repository.GetAllPlayers()
            .Where(p => playerNames.Contains(p.Name))
            .ToDictionary(p => p.Name, p => p.GoalsScored);

        var updatedPlayers = commandService.RecordGoals(playerNames).ToList();

        foreach (var player in updatedPlayers)
        {
            Assert.AreEqual(initialPlayers[player.Name] + 1, player.GoalsScored);
        }
    }

    [TestMethod]
    public void Test_RecordGoals_Integration()
    {
        var provider = TestSetupFactory.CreateServiceProvider(TestMode.Integration);

        var repository = provider.GetRequiredService<IPlayerRepository>();
        var commandService = provider.GetRequiredService<IPlayerCommandService>();

        var playerNames = new List<string> { "Lionel Messi", "Cristiano Ronaldo" };

        var initialPlayers = repository.GetAllPlayers()
            .Where(p => playerNames.Contains(p.Name))
            .ToDictionary(p => p.Name, p => p.GoalsScored);

        commandService.RecordGoals(playerNames);

        var updatedPlayers = repository.GetAllPlayers()
            .Where(p => playerNames.Contains(p.Name))
            .ToDictionary(p => p.Name, p => p.GoalsScored);

        foreach (var name in playerNames)
        {
            Assert.AreEqual(initialPlayers[name] + 1, updatedPlayers[name]);
        }
    }
}