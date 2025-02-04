public class PlayerQueryService : IPlayerQueryService
{
    private readonly IPlayerRepository _repository;

    public PlayerQueryService(IPlayerRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Player>> GetPlayersOlderThanAsync(int age)
    {
        var players = await _repository.GetAllPlayersAsync();
        return players.Where(p => p.Age > age);
    }

    public async Task<IEnumerable<Player>> GetPlayersByNationalityAsync(string nationality)
    {
        var players = await _repository.GetAllPlayersAsync();
        return players.Where(p => p.Nationality.Equals(nationality, StringComparison.OrdinalIgnoreCase));
    }

    public async Task<IEnumerable<string>> GetPlayerNamesAsync()
    {
        var players = await _repository.GetAllPlayersAsync();
        return players.Select(p => p.Name);
    }

    public async Task<int> GetTotalGoalsAsync()
    {
        var players = await _repository.GetAllPlayersAsync();
        return players.Sum(p => p.GoalsScored);
    }

    public async Task<IDictionary<string, List<Player>>> GetPlayersGroupedByNationalityAsync()
    {
        var players = await _repository.GetAllPlayersAsync();
        return players.GroupBy(p => p.Nationality)
                      .ToDictionary(g => g.Key, g => g.ToList());
    }

    public async Task<IEnumerable<Player>> GetPlayersOrderedByPerformanceScoreAsync(bool descending = true)
    {
        var players = await _repository.GetAllPlayersAsync();
        return descending
            ? players.OrderByDescending(p => p.GetPerformanceScore())
            : players.OrderBy(p => p.GetPerformanceScore());
    }

    public async Task<bool> AnyPlayerReachedGoalThresholdAsync(int goalThreshold)
    {
        var players = await _repository.GetAllPlayersAsync();
        return players.Any(p => p.GoalsScored >= goalThreshold);
    }

    public async Task<Player> GetTopPerformerAsync()
    {
        var players = await _repository.GetAllPlayersAsync();
        var top = players.OrderByDescending(p => p.GetPerformanceScore()).FirstOrDefault();
        if (top == null)
            throw new NotFoundException("No players found");
        return top;
    }

    public async Task<IEnumerable<Player>> GetPlayersPagedAsync(int pageNumber, int pageSize)
    {
        var players = await _repository.GetAllPlayersAsync();
        return players.Skip((pageNumber - 1) * pageSize).Take(pageSize);
    }

    public async Task<IEnumerable<string>> GetDistinctNationalitiesAsync()
    {
        var players = await _repository.GetAllPlayersAsync();
        return players.Select(p => p.Nationality).Distinct();
    }
}