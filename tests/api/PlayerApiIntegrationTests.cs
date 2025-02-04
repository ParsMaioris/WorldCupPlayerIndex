[TestClass]
public class PlayerApiIntegrationTests
{
    private static CustomWebApplicationFactory _factory = null!;
    private static HttpClient _client = null!;

    [ClassInitialize]
    public static void Setup(TestContext context)
    {
        _factory = new CustomWebApplicationFactory();
        _client = _factory.CreateClient();
    }

    [TestMethod]
    public async Task GetVeteranPlayers_ReturnsExpectedPlayers()
    {
        var response = await _client.GetAsync("/api/PlayerApplication/veterans");
        response.EnsureSuccessStatusCode();
        var players = await response.Content.ReadFromJsonAsync<List<Player>>() ?? new List<Player>();
        var expectedVeterans = new List<string> { "Cristiano Ronaldo", "Lionel Messi", "Ángel Di María" };
        foreach (var expected in expectedVeterans)
            Assert.IsTrue(players.Any(p => p.Name == expected), $"Expected veteran player '{expected}' to be returned.");
    }
}