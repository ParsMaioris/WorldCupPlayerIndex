using System.Net;

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

    [TestMethod]
    public async Task RecordGoal_NonExistentPlayer_ReturnsNotFound()
    {
        using var factory = new ApiTestFactory(ApiExceptionScenario.NonExistent);
        using var client = factory.CreateAuthorizedClient();
        var response = await client.PostAsync("/api/PlayerCommand/NonExistent/record-goal", null);
        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        Assert.AreEqual("Player 'NonExistent' not found.", errorResponse?.Error);
    }

    [TestMethod]
    public async Task RecordGoal_NonVeteranPlayer_ReturnsForbidden()
    {
        using var factory = new ApiTestFactory(ApiExceptionScenario.NonVeteran);
        using var client = factory.CreateAuthorizedClient();
        var response = await client.PostAsync("/api/PlayerCommand/SomePlayer/record-goal", null);
        Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode);
        var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        Assert.AreEqual("Only veteran players can record goals.", errorResponse?.Error);
    }

    [TestMethod]
    public async Task RecordGoal_EmptyPlayerName_ReturnsBadRequest()
    {
        using var factory = new ApiTestFactory(ApiExceptionScenario.Default);
        using var client = factory.CreateAuthorizedClient();
        var encodedPlayerName = Uri.EscapeDataString(" ");
        var response = await client.PostAsync($"/api/PlayerCommand/{encodedPlayerName}/record-goal", null);
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        Assert.AreEqual("The playerName field is required.", errorResponse?.Error);
    }
}