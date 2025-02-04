public class PlayerQueryService : IPlayerQueryService
{
    private readonly IPlayerRepository _repository;

    public PlayerQueryService(IPlayerRepository repository)
    {
        _repository = repository;
    }

    public IEnumerable<Player> GetPlayersOlderThan(int age) =>
        _repository.GetAllPlayers().Where(p => p.Age > age);

    public IEnumerable<Player> GetPlayersByNationality(string nationality) =>
        _repository.GetAllPlayers().Where(p => p.Nationality.Equals(nationality, StringComparison.OrdinalIgnoreCase));

    public IEnumerable<string> GetPlayerNames() =>
        _repository.GetAllPlayers().Select(p => p.Name);

    public int GetTotalGoals() =>
        _repository.GetAllPlayers().Sum(p => p.GoalsScored);

    public IEnumerable<IGrouping<string, Player>> GetPlayersGroupedByNationality() =>
        _repository.GetAllPlayers().GroupBy(p => p.Nationality);

    public IEnumerable<Player> GetPlayersOrderedByPerformanceScore(bool descending = true) =>
        descending
            ? _repository.GetAllPlayers().OrderByDescending(p => p.GetPerformanceScore())
            : _repository.GetAllPlayers().OrderBy(p => p.GetPerformanceScore());

    public bool AnyPlayerReachedGoalThreshold(int goalThreshold) =>
        _repository.GetAllPlayers().Any(p => p.GoalsScored >= goalThreshold);

    public Player GetTopPerformer()
    {
        var top = _repository.GetAllPlayers().OrderByDescending(p => p.GetPerformanceScore()).FirstOrDefault();
        if (top == null)
            throw new NotFoundException("No players found");
        return top;
    }

    public IEnumerable<Player> GetPlayersPaged(int pageNumber, int pageSize) =>
        _repository.GetAllPlayers().Skip((pageNumber - 1) * pageSize).Take(pageSize);

    public IEnumerable<string> GetDistinctNationalities() =>
        _repository.GetAllPlayers().Select(p => p.Nationality).Distinct();
}