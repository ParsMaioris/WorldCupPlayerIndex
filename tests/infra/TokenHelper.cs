public static class TokenHelper
{
    public static async Task<string> GetTokenAsync(HttpClient client)
    {
        var response = await client.GetAsync("/api/authentication");
        response.EnsureSuccessStatusCode();
        var tokenResponse = await response.Content.ReadFromJsonAsync<TokenResponse>();
        if (string.IsNullOrWhiteSpace(tokenResponse?.Token))
            throw new InvalidOperationException("Token is null or empty.");
        return tokenResponse.Token;
    }

    private class TokenResponse
    {
        public string Token { get; set; } = string.Empty;
    }
}