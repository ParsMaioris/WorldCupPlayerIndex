public class PlayerQueryService : IPlayerQueryService
{
    private readonly IPlayerRepository _repository;

    public PlayerQueryService(IPlayerRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Player>> GetPlayersOlderThanAsync(int age)
    {
        if (age < 0)
            throw new QueryInvalidException("Age cannot be negative.");

        var players = await _repository.GetAllPlayersAsync();
        var result = players.Where(p => p.Age > age).ToList();

        if (!result.Any())
            throw new QueryNotFoundException($"No players older than {age} found.");

        return result;
    }

    public async Task<IEnumerable<Player>> GetPlayersByNationalityAsync(string nationality)
    {
        if (string.IsNullOrWhiteSpace(nationality))
            throw new QueryInvalidException("Nationality cannot be empty.");

        var players = await _repository.GetAllPlayersAsync();
        var result = players.Where(p => p.Nationality.Equals(nationality, StringComparison.OrdinalIgnoreCase)).ToList();

        if (!result.Any())
            throw new QueryNotFoundException($"No players found from {nationality}.");

        return result;
    }

    public async Task<IEnumerable<string>> GetPlayerNamesAsync()
    {
        var players = await _repository.GetAllPlayersAsync();
        if (!players.Any())
            throw new QueryNotFoundException("No players found.");

        return players.Select(p => p.Name);
    }

    public async Task<int> GetTotalGoalsAsync()
    {
        var players = await _repository.GetAllPlayersAsync();
        if (!players.Any())
            throw new QueryNotFoundException("No players found to calculate total goals.");

        return players.Sum(p => p.GoalsScored);
    }

    public async Task<IDictionary<string, List<Player>>> GetPlayersGroupedByNationalityAsync()
    {
        var players = await _repository.GetAllPlayersAsync();
        if (!players.Any())
            throw new QueryNotFoundException("No players found to group by nationality.");

        return players.GroupBy(p => p.Nationality)
                      .ToDictionary(g => g.Key, g => g.ToList());
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

    public async Task<bool> AnyPlayerReachedGoalThresholdAsync(int goalThreshold)
    {
        if (goalThreshold < 0)
            throw new QueryInvalidException("Goal threshold cannot be negative.");

        var players = await _repository.GetAllPlayersAsync();
        if (!players.Any())
            throw new QueryNotFoundException("No players found to check goal threshold.");

        return players.Any(p => p.GoalsScored >= goalThreshold);
    }

    public async Task<Player> GetTopPerformerAsync()
    {
        var players = await _repository.GetAllPlayersAsync();
        var top = players.OrderByDescending(p => p.GetPerformanceScore()).FirstOrDefault();

        if (top == null)
            throw new QueryNotFoundException("No players found.");

        return top;
    }

    public async Task<IEnumerable<Player>> GetPlayersPagedAsync(int pageNumber, int pageSize)
    {
        if (pageNumber <= 0 || pageSize <= 0)
            throw new QueryInvalidException("Page number and page size must be greater than zero.");

        var players = await _repository.GetAllPlayersAsync();
        var result = players.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

        if (!result.Any())
            throw new QueryNotFoundException("No players found for the given page.");

        return result;
    }

    public async Task<IEnumerable<string>> GetDistinctNationalitiesAsync()
    {
        var players = await _repository.GetAllPlayersAsync();
        if (!players.Any())
            throw new QueryNotFoundException("No players found to extract distinct nationalities.");

        return players.Select(p => p.Nationality).Distinct();
    }
}