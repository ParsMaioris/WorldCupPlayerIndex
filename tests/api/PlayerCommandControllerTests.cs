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

    [DataTestMethod]
    [DataRow(ApiExceptionScenario.NonExistent, "NonExistent", HttpStatusCode.NotFound, "Player 'NonExistent' not found.")]
    [DataRow(ApiExceptionScenario.NonVeteran, "SomePlayer", HttpStatusCode.Forbidden, "Only veteran players can record goals.")]
    public async Task RecordGoal_ParameterizedTests(ApiExceptionScenario scenario, string playerName, HttpStatusCode expectedStatusCode, string expectedError)
    {
        using var factory = new ApiTestFactory(scenario);
        using var client = factory.CreateAuthorizedClient();
        var encodedPlayerName = Uri.EscapeDataString(playerName);
        var response = await client.PostAsync($"/api/PlayerCommand/{encodedPlayerName}/record-goal", null);
        Assert.AreEqual(expectedStatusCode, response.StatusCode);
        var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        Assert.AreEqual(expectedError, errorResponse?.Error);
    }

    [DataTestMethod]
    [DataRow(ApiExceptionScenario.Default, "", HttpStatusCode.BadRequest, "Player names list cannot be empty.")]
    [DataRow(ApiExceptionScenario.NonExistent, "NonExistent1,NonExistent2", HttpStatusCode.NotFound, "None of the specified players were found.")]
    [DataRow(ApiExceptionScenario.NonVeteran, "SomePlayer", HttpStatusCode.Forbidden, "Some players are not allowed to have goals recorded.")]
    public async Task RecordGoals_ExceptionScenarios(ApiExceptionScenario scenario, string playerNamesCsv, HttpStatusCode expectedStatusCode, string expectedError)
    {
        IEnumerable<string> playerNames = string.IsNullOrWhiteSpace(playerNamesCsv)
            ? new List<string>()
            : playerNamesCsv.Split(',').Select(n => n.Trim()).ToList();

        using var factory = new ApiTestFactory(scenario);
        using var client = factory.CreateAuthorizedClient();
        var content = JsonContent.Create(playerNames);
        var response = await client.PostAsync("/api/PlayerCommand/record-goals", content);
        Assert.AreEqual(expectedStatusCode, response.StatusCode);
        var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        Assert.AreEqual(expectedError, errorResponse?.Error);
    }
}