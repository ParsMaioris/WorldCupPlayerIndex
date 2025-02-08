using System.Net.Http.Headers;

[TestClass]
public class PlayerApplicationControllerTests
{
    private CustomWebApplicationFactory? _factory;
    private HttpClient? _client;

    [TestInitialize]
    public void Setup()
    {
        _factory = new CustomWebApplicationFactory();
        _client = _factory.CreateClient();
        var token = TokenHelper.GetTokenAsync(_client!).GetAwaiter().GetResult();
        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
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
        var response = await _client!.GetAsync("/api/PlayerApplication/veterans");
        response.EnsureSuccessStatusCode();
        var players = await response.Content.ReadFromJsonAsync<List<Player>>() ?? new List<Player>();
        var expectedNames = new List<string> { "Cristiano Ronaldo", "Lionel Messi", "Ángel Di María" };

        foreach (var name in expectedNames)
        {
            Assert.IsTrue(players.Any(p => p.Name == name));
        }
    }
}