[TestClass]
public class PlayerQueryServiceTests
{
    [DataTestMethod]
    [DataRow(TestMode.Unit)]
    [DataRow(TestMode.Integration)]
    public async Task Test_GetPlayersOlderThan(TestMode mode)
    {
        var provider = TestSetupFactory.CreateServiceProvider(mode);
        var queryService = provider.GetRequiredService<IPlayerQueryService>();

        var players = (await queryService.GetPlayersOlderThanAsync(30)).ToList();

        Assert.IsTrue(players.Any(p => p.Name == "Cristiano Ronaldo"));
        Assert.IsTrue(players.Any(p => p.Name == "Lionel Messi"));
        Assert.IsTrue(players.Any(p => p.Name == "Robert Lewandowski"));
        Assert.IsFalse(players.Any(p => p.Name == "Neymar Jr"));
    }

    [DataTestMethod]
    [DataRow(TestMode.Unit)]
    [DataRow(TestMode.Integration)]
    public async Task Test_GetPlayersByNationality(TestMode mode)
    {
        var provider = TestSetupFactory.CreateServiceProvider(mode);
        var queryService = provider.GetRequiredService<IPlayerQueryService>();

        var portuguesePlayers = await queryService.GetPlayersByNationalityAsync("Portugal");

        Assert.IsTrue(portuguesePlayers.Any(p => p.Name == "Cristiano Ronaldo"), "Expected to find Cristiano Ronaldo.");
        Assert.IsTrue(portuguesePlayers.Any(p => p.Name == "Bruno Fernandes"), "Expected to find Bruno Fernandes.");
        Assert.IsTrue(portuguesePlayers.All(p => p.Nationality.Equals("Portugal", StringComparison.OrdinalIgnoreCase)));
    }

    [DataTestMethod]
    [DataRow(TestMode.Unit)]
    [DataRow(TestMode.Integration)]
    public async Task Test_GetPlayerNames(TestMode mode)
    {
        var provider = TestSetupFactory.CreateServiceProvider(mode);
        var queryService = provider.GetRequiredService<IPlayerQueryService>();

        var names = (await queryService.GetPlayerNamesAsync()).ToList();
        var expectedCount = SeedData.CreateSamplePlayers().Count();

        Assert.AreEqual(expectedCount, names.Count);
    }

    [DataTestMethod]
    [DataRow(TestMode.Unit)]
    [DataRow(TestMode.Integration)]
    public async Task Test_GetTotalGoals(TestMode mode)
    {
        var provider = TestSetupFactory.CreateServiceProvider(mode);
        var queryService = provider.GetRequiredService<IPlayerQueryService>();

        var totalGoals = await queryService.GetTotalGoalsAsync();
        var expectedTotal = SeedData.CreateSamplePlayers().Sum(p => p.GoalsScored);

        Assert.AreEqual(expectedTotal, totalGoals);
    }

    [DataTestMethod]
    [DataRow(TestMode.Unit)]
    [DataRow(TestMode.Integration)]
    public async Task Test_GetPlayersGroupedByNationality(TestMode mode)
    {
        var provider = TestSetupFactory.CreateServiceProvider(mode);
        var queryService = provider.GetRequiredService<IPlayerQueryService>();

        var groups = (await queryService.GetPlayersGroupedByNationalityAsync()).ToList();
        var expectedGroups = SeedData.CreateSamplePlayers().GroupBy(p => p.Nationality);

        Assert.AreEqual(expectedGroups.Count(), groups.Count());
    }

    [DataTestMethod]
    [DataRow(TestMode.Unit)]
    [DataRow(TestMode.Integration)]
    public async Task Test_GetPlayersOrderedByPerformanceScore(TestMode mode)
    {
        var provider = TestSetupFactory.CreateServiceProvider(mode);
        var queryService = provider.GetRequiredService<IPlayerQueryService>();

        var orderedDesc = (await queryService.GetPlayersOrderedByPerformanceScoreAsync(true)).ToList();
        var orderedAsc = (await queryService.GetPlayersOrderedByPerformanceScoreAsync(false)).ToList();

        Assert.IsTrue(orderedDesc.First().GetPerformanceScore() >= orderedDesc.Last().GetPerformanceScore());
        Assert.IsTrue(orderedAsc.First().GetPerformanceScore() <= orderedAsc.Last().GetPerformanceScore());
    }

    [DataTestMethod]
    [DataRow(TestMode.Unit)]
    [DataRow(TestMode.Integration)]
    public async Task Test_AnyPlayerReachedGoalThreshold(TestMode mode)
    {
        var provider = TestSetupFactory.CreateServiceProvider(mode);
        var queryService = provider.GetRequiredService<IPlayerQueryService>();

        Assert.IsTrue(await queryService.AnyPlayerReachedGoalThresholdAsync(100));
        Assert.IsFalse(await queryService.AnyPlayerReachedGoalThresholdAsync(1000));
    }

    [DataTestMethod]
    [DataRow(TestMode.Unit)]
    [DataRow(TestMode.Integration)]
    public async Task Test_GetTopPerformer(TestMode mode)
    {
        var provider = TestSetupFactory.CreateServiceProvider(mode);
        var queryService = provider.GetRequiredService<IPlayerQueryService>();

        var top = await queryService.GetTopPerformerAsync();
        var repository = provider.GetRequiredService<IPlayerRepository>();
        var expectedTop = (await repository.GetAllPlayersAsync())
            .OrderByDescending(p => p.GetPerformanceScore())
            .First();

        Assert.AreEqual(expectedTop.Name, top.Name);
    }

    [DataTestMethod]
    [DataRow(TestMode.Unit)]
    [DataRow(TestMode.Integration)]
    public async Task Test_GetPlayersPaged(TestMode mode)
    {
        var provider = TestSetupFactory.CreateServiceProvider(mode);
        var queryService = provider.GetRequiredService<IPlayerQueryService>();

        var repository = provider.GetRequiredService<IPlayerRepository>();
        var allPlayers = (await repository.GetAllPlayersAsync()).ToList();

        int pageSize = 5;
        var page1 = (await queryService.GetPlayersPagedAsync(1, pageSize)).ToList();
        var page2 = (await queryService.GetPlayersPagedAsync(2, pageSize)).ToList();

        Assert.AreEqual(pageSize, page1.Count);
        Assert.IsTrue(page1.All(p => allPlayers.Contains(p)));
        Assert.IsTrue(page2.All(p => allPlayers.Contains(p)));
        Assert.IsFalse(page1.Intersect(page2).Any());
    }

    [DataTestMethod]
    [DataRow(TestMode.Unit)]
    [DataRow(TestMode.Integration)]
    public async Task Test_GetDistinctNationalities(TestMode mode)
    {
        var provider = TestSetupFactory.CreateServiceProvider(mode);
        var queryService = provider.GetRequiredService<IPlayerQueryService>();

        var distinct = (await queryService.GetDistinctNationalitiesAsync()).ToList();
        var expectedDistinct = SeedData.CreateSamplePlayers().Select(p => p.Nationality).Distinct().ToList();

        Assert.AreEqual(expectedDistinct.Count, distinct.Count());
        foreach (var nat in expectedDistinct)
        {
            Assert.IsTrue(distinct.Contains(nat));
        }
    }
}