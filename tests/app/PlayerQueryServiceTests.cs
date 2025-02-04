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

        Assert.IsTrue(portuguesePlayers.All(p => p.Nationality.Equals("Portugal", StringComparison.OrdinalIgnoreCase)));
    }

    [DataTestMethod]
    [DataRow(TestMode.Unit)]
    [DataRow(TestMode.Integration)]
    public void Test_GetPlayerNames(TestMode mode)
    {
        var provider = TestSetupFactory.CreateServiceProvider(mode);

        var queryService = provider.GetRequiredService<IPlayerQueryService>();

        var names = queryService.GetPlayerNames().ToList();

        var expectedNames = SeedData.CreateSamplePlayers().Select(p => p.Name).ToList();

        Assert.AreEqual(expectedNames.Count, names.Count());

        foreach (var name in expectedNames)
        {
            Assert.IsTrue(names.Contains(name));
        }
    }

    [DataTestMethod]
    [DataRow(TestMode.Unit)]
    [DataRow(TestMode.Integration)]
    public void Test_GetTotalGoals(TestMode mode)
    {
        var provider = TestSetupFactory.CreateServiceProvider(mode);

        var queryService = provider.GetRequiredService<IPlayerQueryService>();

        var totalGoals = queryService.GetTotalGoals();

        var expectedTotal = SeedData.CreateSamplePlayers().Sum(p => p.GoalsScored);

        Assert.AreEqual(expectedTotal, totalGoals);
    }

    [DataTestMethod]
    [DataRow(TestMode.Unit)]
    [DataRow(TestMode.Integration)]
    public void Test_GetPlayersGroupedByNationality(TestMode mode)
    {
        var provider = TestSetupFactory.CreateServiceProvider(mode);

        var queryService = provider.GetRequiredService<IPlayerQueryService>();

        var groups = queryService.GetPlayersGroupedByNationality().ToList();

        var expectedGroups = SeedData.CreateSamplePlayers().GroupBy(p => p.Nationality);

        Assert.AreEqual(expectedGroups.Count(), groups.Count());
    }

    [DataTestMethod]
    [DataRow(TestMode.Unit)]
    [DataRow(TestMode.Integration)]
    public void Test_GetPlayersOrderedByPerformanceScore(TestMode mode)
    {
        var provider = TestSetupFactory.CreateServiceProvider(mode);

        var queryService = provider.GetRequiredService<IPlayerQueryService>();

        var orderedDesc = queryService.GetPlayersOrderedByPerformanceScore(true).ToList();
        var orderedAsc = queryService.GetPlayersOrderedByPerformanceScore(false).ToList();

        Assert.IsTrue(orderedDesc.First().GetPerformanceScore() >= orderedDesc.Last().GetPerformanceScore());
        Assert.IsTrue(orderedAsc.First().GetPerformanceScore() <= orderedAsc.Last().GetPerformanceScore());
    }

    [DataTestMethod]
    [DataRow(TestMode.Unit)]
    [DataRow(TestMode.Integration)]
    public void Test_AnyPlayerReachedGoalThreshold(TestMode mode)
    {
        var provider = TestSetupFactory.CreateServiceProvider(mode);

        var queryService = provider.GetRequiredService<IPlayerQueryService>();

        Assert.IsTrue(queryService.AnyPlayerReachedGoalThreshold(100));
        Assert.IsFalse(queryService.AnyPlayerReachedGoalThreshold(1000));
    }

    [DataTestMethod]
    [DataRow(TestMode.Unit)]
    [DataRow(TestMode.Integration)]
    public void Test_GetTopPerformer(TestMode mode)
    {
        var provider = TestSetupFactory.CreateServiceProvider(mode);

        var queryService = provider.GetRequiredService<IPlayerQueryService>();

        var top = queryService.GetTopPerformer();

        var expectedTop = provider.GetRequiredService<IPlayerRepository>().GetAllPlayers()
            .OrderByDescending(p => p.GetPerformanceScore())
            .First();

        Assert.AreEqual(expectedTop.Name, top.Name);
    }

    [DataTestMethod]
    [DataRow(TestMode.Unit)]
    [DataRow(TestMode.Integration)]
    public void Test_GetPlayersPaged(TestMode mode)
    {
        var provider = TestSetupFactory.CreateServiceProvider(mode);

        var queryService = provider.GetRequiredService<IPlayerQueryService>();

        var allPlayers = provider.GetRequiredService<IPlayerRepository>().GetAllPlayers().ToList();

        int pageSize = 5;

        var page1 = queryService.GetPlayersPaged(1, pageSize).ToList();
        var page2 = queryService.GetPlayersPaged(2, pageSize).ToList();

        Assert.AreEqual(pageSize, page1.Count);
        Assert.IsTrue(page1.All(p => allPlayers.Contains(p)));
        Assert.IsTrue(page2.All(p => allPlayers.Contains(p)));
        Assert.IsFalse(page1.Intersect(page2).Any());
    }

    [DataTestMethod]
    [DataRow(TestMode.Unit)]
    [DataRow(TestMode.Integration)]
    public void Test_GetDistinctNationalities(TestMode mode)
    {
        var provider = TestSetupFactory.CreateServiceProvider(mode);

        var queryService = provider.GetRequiredService<IPlayerQueryService>();

        var distinct = queryService.GetDistinctNationalities().ToList();

        var expectedDistinct = SeedData.CreateSamplePlayers().Select(p => p.Nationality).Distinct().ToList();

        Assert.AreEqual(expectedDistinct.Count, distinct.Count());

        foreach (var nat in expectedDistinct)
        {
            Assert.IsTrue(distinct.Contains(nat));
        }
    }
}