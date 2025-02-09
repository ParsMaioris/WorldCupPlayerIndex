using System.Net;

[TestClass]
public class PlayerDomainControllerExceptionFilterTests
{
    [TestMethod]
    public async Task GetVeteranPlayers_NoVeteransFound_ReturnsNotFound()
    {
        using var factory = new ApiTestFactory(ApiExceptionScenario.NoPlayersExist);
        using var client = factory.CreateAuthorizedClient();
        var response = await client.GetAsync("/api/PlayerDomain/veterans");
        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode, "Expected NotFound when no veteran players exist.");
        var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        Assert.AreEqual("No veteran players found.", errorResponse?.Error);
    }

    [TestMethod]
    public async Task GetPlayersOrderedByPerformance_NoPlayersForRanking_ReturnsNotFound()
    {
        using var factory = new ApiTestFactory(ApiExceptionScenario.NoPlayersExist);
        using var client = factory.CreateAuthorizedClient();
        var response = await client.GetAsync("/api/PlayerDomain/ordered-by-performance?descending=true");
        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode, "Expected NotFound when no players are available for ranking.");
        var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        Assert.AreEqual("No players are available for performance ranking.", errorResponse?.Error);
    }
}