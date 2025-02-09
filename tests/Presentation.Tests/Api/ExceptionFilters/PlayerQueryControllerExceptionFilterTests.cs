using System.Net;

[TestClass]
public class PlayerQueryControllerExceptionFilterTests
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

    [DataTestMethod]
    [DataRow(-1, HttpStatusCode.BadRequest, "Age cannot be negative.")]
    [DataRow(200, HttpStatusCode.NotFound, "No players older than 200 found.")]
    public async Task GetPlayersOlderThan_ExceptionScenarios(int age, HttpStatusCode expectedStatus, string expectedError)
    {
        var response = await _client!.GetAsync($"/api/PlayerQuery/older-than/{age}");
        Assert.AreEqual(expectedStatus, response.StatusCode);
        var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        Assert.AreEqual(expectedError, errorResponse?.Error);
    }

    [DataTestMethod]
    [DataRow(" ", HttpStatusCode.BadRequest, "The nationality field is required.")]
    [DataRow("Martian", HttpStatusCode.NotFound, "No players found from Martian.")]
    public async Task GetPlayersByNationality_ExceptionScenarios(string nationality, HttpStatusCode expectedStatus, string expectedError)
    {
        var encodedNationality = Uri.EscapeDataString(nationality);
        var response = await _client!.GetAsync($"/api/PlayerQuery/nationality/{encodedNationality}");
        Assert.AreEqual(expectedStatus, response.StatusCode);
        var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        Assert.AreEqual(expectedError, errorResponse?.Error);
    }

    [DataTestMethod]
    [DataRow(-10, HttpStatusCode.BadRequest, "Goal threshold cannot be negative.")]
    public async Task AnyPlayerReached_ExceptionScenarios(int threshold, HttpStatusCode expectedStatus, string expectedError)
    {
        var response = await _client!.GetAsync($"/api/PlayerQuery/any-player-reached?threshold={threshold}");
        Assert.AreEqual(expectedStatus, response.StatusCode);
        var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        Assert.AreEqual(expectedError, errorResponse?.Error);
    }

    [DataTestMethod]
    [DataRow(0, 5, HttpStatusCode.BadRequest, "Page number and page size must be greater than zero.")]
    [DataRow(100, 5, HttpStatusCode.NotFound, "No players found for the given page.")]
    public async Task GetPlayersPaged_ExceptionScenarios(int pageNumber, int pageSize, HttpStatusCode expectedStatus, string expectedError)
    {
        var response = await _client!.GetAsync($"/api/PlayerQuery/paged?pageNumber={pageNumber}&pageSize={pageSize}");
        Assert.AreEqual(expectedStatus, response.StatusCode);
        var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();
        Assert.AreEqual(expectedError, errorResponse?.Error);
    }
}