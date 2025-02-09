using System.Net;
using System.Net.Http.Headers;

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
            Assert.IsTrue(players.Any(p => p.Name == name));
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

        Assert.IsTrue(asc.First().GetPerformanceScore() <= asc.Last().GetPerformanceScore());
        Assert.IsTrue(desc.First().GetPerformanceScore() >= desc.Last().GetPerformanceScore());
    }

    [TestMethod]
    public async Task GetVeteranPlayers_NoVeteransFound_ReturnsNotFound()
    {
        using var factory = new ApiTestFactory(players => players.Where(p => p.Age < 35));
        using var client = factory.CreateAuthorizedClient();
        var response = await client.GetAsync("/api/PlayerDomain/veterans");
        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        Assert.AreEqual("No veteran players found.", errorResponse?.Error);
    }

    [TestMethod]
    public async Task GetPlayersOrderedByPerformance_NoPlayersForRanking_ReturnsNotFound()
    {
        using var factory = new ApiTestFactory(players => Enumerable.Empty<Player>());
        using var client = factory.CreateAuthorizedClient();
        var response = await client.GetAsync("/api/PlayerDomain/ordered-by-performance?descending=true");
        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        Assert.AreEqual("No players are available for performance ranking.", errorResponse?.Error);
    }
}