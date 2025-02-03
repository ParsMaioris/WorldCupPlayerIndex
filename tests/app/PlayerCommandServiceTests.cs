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

        Assert.AreEqual(initialGoalCount + 1, messi.GoalsScored);
    }
}