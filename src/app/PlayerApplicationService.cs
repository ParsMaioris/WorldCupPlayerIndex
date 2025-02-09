public class PlayerApplicationService : IPlayerApplicationService
{
    private readonly IPlayerDomainService _playerDomainService;

    public PlayerApplicationService(IPlayerDomainService playerDomainService)
    {
        _playerDomainService = playerDomainService;
    }

    public async Task<IEnumerable<Player>> GetVeteranPlayersAsync()
    {
        return await _playerDomainService.GetVeteranPlayersAsync();
    }

    public async Task<IEnumerable<Player>> GetPlayersOrderedByPerformanceScoreAsync(bool descending = true)
    {
        return await _playerDomainService.GetPlayersOrderedByPerformanceScoreAsync(descending);
    }
}