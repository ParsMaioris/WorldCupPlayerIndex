[TestClass]
public class PlayerDomainControllerTests
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
    public async Task GetVeteranPlayers_ReturnsExpectedPlayers()
    {
        var response = await _client!.GetAsync("/api/PlayerDomain/veterans");
        response.EnsureSuccessStatusCode();
        var players = await response.Content.ReadFromJsonAsync<List<Player>>() ?? new List<Player>();
        var expectedNames = new List<string> { "Cristiano Ronaldo", "Lionel Messi", "Ángel Di María" };

        foreach (var name in expectedNames)
        {
            Assert.IsTrue(players.Any(p => p.Name == name), $"Expected player '{name}' was not found.");
        }
    }

    [TestMethod]
    public async Task GetPlayersOrderedByPerformance_ReturnsSortedPlayers()
    {
        var responseAsc = await _client!.GetAsync("/api/PlayerDomain/ordered-by-performance?descending=false");
        responseAsc.EnsureSuccessStatusCode();
        var asc = await responseAsc.Content.ReadFromJsonAsync<List<Player>>() ?? new List<Player>();

        var responseDesc = await _client.GetAsync("/api/PlayerDomain/ordered-by-performance?descending=true");
        responseDesc.EnsureSuccessStatusCode();
        var desc = await responseDesc.Content.ReadFromJsonAsync<List<Player>>() ?? new List<Player>();

        Assert.IsTrue(asc.First().GetPerformanceScore() <= asc.Last().GetPerformanceScore(),
            "Ascending order test failed: first player's performance is higher than the last player's.");
        Assert.IsTrue(desc.First().GetPerformanceScore() >= desc.Last().GetPerformanceScore(),
            "Descending order test failed: first player's performance is lower than the last player's.");
    }
}