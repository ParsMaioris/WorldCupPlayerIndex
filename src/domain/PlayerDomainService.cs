public class PlayerDomainService : IPlayerDomainService
{
    private readonly IPlayerRepository _repository;

    public PlayerDomainService(IPlayerRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Player>> GetVeteranPlayersAsync()
    {
        var players = await _repository.GetAllPlayersAsync();
        var veterans = players.Where(p => p.IsVeteran()).ToList();
        if (!veterans.Any())
        {
            throw new NoVeteranPlayersFoundException();
        }
        return veterans;
    }

    public async Task<IEnumerable<Player>> GetPlayersOrderedByPerformanceScoreAsync(bool descending = true)
    {
        var players = await _repository.GetAllPlayersAsync();
        if (!players.Any())
            throw new QueryNotFoundException("No players found to order by performance score.");

        return descending
            ? players.OrderByDescending(p => p.GetPerformanceScore())
            : players.OrderBy(p => p.GetPerformanceScore());
    }
}