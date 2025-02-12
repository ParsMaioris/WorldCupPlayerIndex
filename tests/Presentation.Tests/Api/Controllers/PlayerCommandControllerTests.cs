[TestClass]
public class PlayerCommandControllerTests
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
    public async Task RecordGoal_ReturnsUpdatedPlayer()
    {
        var response = await _client!.PostAsync("/api/PlayerCommand/Lionel Messi/record-goal", null);
        response.EnsureSuccessStatusCode();
        var player = await response.Content.ReadFromJsonAsync<Player>();
        Assert.IsNotNull(player);
        Assert.AreEqual(751, player!.GoalsScored);
    }

    [TestMethod]
    public async Task RecordGoals_ReturnsUpdatedPlayers()
    {
        var names = new List<string> { "Lionel Messi", "Cristiano Ronaldo" };
        var content = JsonContent.Create(names);
        var response = await _client!.PostAsync("/api/PlayerCommand/record-goals", content);
        response.EnsureSuccessStatusCode();
        var players = await response.Content.ReadFromJsonAsync<List<Player>>() ?? new List<Player>();
        Assert.IsTrue(players.Exists(p => p.Name == "Lionel Messi" && p.GoalsScored == 751));
        Assert.IsTrue(players.Exists(p => p.Name == "Cristiano Ronaldo" && p.GoalsScored == 801));
    }
}