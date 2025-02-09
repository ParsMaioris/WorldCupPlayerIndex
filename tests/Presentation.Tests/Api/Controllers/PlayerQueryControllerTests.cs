using System.Text.Json;

[TestClass]
public class PlayerQueryControllerTests
{
    private ApiTestFactory? _factory;
    private HttpClient? _client;

    [TestInitialize]
    public void Setup()
    {
        _factory = new ApiTestFactory();
        _client = _factory.CreateAuthorizedClient();
    }

    [TestCleanup]
    public void Cleanup()
    {
        _client?.Dispose();
        _factory?.Dispose();
    }

    [TestMethod]
    public async Task GetPlayersOlderThan_ReturnsCorrectPlayers()
    {
        var response = await _client!.GetAsync("/api/PlayerQuery/older-than/30");
        response.EnsureSuccessStatusCode();
        var players = await response.Content.ReadFromJsonAsync<List<Player>>() ?? new List<Player>();

        Assert.IsTrue(players.Any(p => p.Name == "Cristiano Ronaldo"));
        Assert.IsTrue(players.Any(p => p.Name == "Lionel Messi"));
        Assert.IsFalse(players.Any(p => p.Name == "Kylian Mbappe"));
    }

    [TestMethod]
    public async Task GetPlayersByNationality_ReturnsCorrectPlayers()
    {
        var response = await _client!.GetAsync("/api/PlayerQuery/nationality/Portugal");
        response.EnsureSuccessStatusCode();
        var players = await response.Content.ReadFromJsonAsync<List<Player>>() ?? new List<Player>();

        Assert.IsTrue(players.All(p => p.Nationality.ToLower() == "portugal"));
        Assert.IsTrue(players.Any(p => p.Name == "Bruno Fernandes"));
    }

    [TestMethod]
    public async Task GetPlayerNames_ReturnsAllNames()
    {
        var response = await _client!.GetAsync("/api/PlayerQuery/names");
        response.EnsureSuccessStatusCode();
        var names = await response.Content.ReadFromJsonAsync<List<string>>() ?? new List<string>();
        var expectedCount = SeedData.CreateSamplePlayers().Count();

        Assert.AreEqual(expectedCount, names.Count);
    }

    [TestMethod]
    public async Task GetTotalGoals_ReturnsCorrectSum()
    {
        var response = await _client!.GetAsync("/api/PlayerQuery/total-goals");
        response.EnsureSuccessStatusCode();
        var total = await response.Content.ReadFromJsonAsync<int>();
        var expectedTotal = SeedData.CreateSamplePlayers().Sum(p => p.GoalsScored);

        Assert.AreEqual(expectedTotal, total);
    }

    [TestMethod]
    public async Task GetPlayersGroupedByNationality_ReturnsGroups()
    {
        var response = await _client!.GetAsync("/api/PlayerQuery/grouped-by-nationality");
        response.EnsureSuccessStatusCode();
        using var document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        var root = document.RootElement;

        Assert.AreEqual(JsonValueKind.Object, root.ValueKind);

        int groupCount = root.EnumerateObject().Count();
        var expectedCount = SeedData.CreateSamplePlayers().GroupBy(p => p.Nationality).Count();

        Assert.AreEqual(expectedCount, groupCount);
    }

    [TestMethod]
    public async Task AnyPlayerReachedGoalThreshold_ReturnsCorrectBool()
    {
        var responseTrue = await _client!.GetAsync("/api/PlayerQuery/any-player-reached?threshold=100");
        responseTrue.EnsureSuccessStatusCode();
        var resultTrue = await responseTrue.Content.ReadFromJsonAsync<bool>();

        Assert.IsTrue(resultTrue);

        var responseFalse = await _client.GetAsync("/api/PlayerQuery/any-player-reached?threshold=1000");
        responseFalse.EnsureSuccessStatusCode();
        var resultFalse = await responseFalse.Content.ReadFromJsonAsync<bool>();

        Assert.IsFalse(resultFalse);
    }

    [TestMethod]
    public async Task GetTopPerformer_ReturnsExpectedPlayer()
    {
        var response = await _client!.GetAsync("/api/PlayerQuery/top-performer");
        response.EnsureSuccessStatusCode();
        var top = await response.Content.ReadFromJsonAsync<Player>();
        var expected = SeedData.CreateSamplePlayers().OrderByDescending(p => p.GetPerformanceScore()).First();

        Assert.AreEqual(expected.Name, top?.Name);
    }

    [TestMethod]
    public async Task GetPlayersPaged_ReturnsCorrectPage()
    {
        var response = await _client!.GetAsync("/api/PlayerQuery/paged?pageNumber=1&pageSize=5");
        response.EnsureSuccessStatusCode();
        var json = await response.Content.ReadAsStringAsync();
        var players = JsonSerializer.Deserialize<List<Player>>(json,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<Player>();

        Assert.AreEqual(5, players.Count);
    }

    [TestMethod]
    public async Task GetDistinctNationalities_ReturnsUniqueList()
    {
        var response = await _client!.GetAsync("/api/PlayerQuery/distinct-nationalities");
        response.EnsureSuccessStatusCode();
        var nations = await response.Content.ReadFromJsonAsync<List<string>>() ?? new List<string>();
        var expectedCount = SeedData.CreateSamplePlayers().Select(p => p.Nationality).Distinct().Count();

        Assert.AreEqual(expectedCount, nations.Count);
    }
}