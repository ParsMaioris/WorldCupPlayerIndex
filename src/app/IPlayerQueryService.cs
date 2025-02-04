public interface IPlayerQueryService
{
    Task<IEnumerable<Player>> GetPlayersOlderThanAsync(int age);
    Task<IEnumerable<Player>> GetPlayersByNationalityAsync(string nationality);
    Task<IEnumerable<string>> GetPlayerNamesAsync();
    Task<int> GetTotalGoalsAsync();
    Task<IDictionary<string, List<Player>>> GetPlayersGroupedByNationalityAsync();
    Task<IEnumerable<Player>> GetPlayersOrderedByPerformanceScoreAsync(bool descending = true);
    Task<bool> AnyPlayerReachedGoalThresholdAsync(int goalThreshold);
    Task<Player> GetTopPerformerAsync();
    Task<IEnumerable<Player>> GetPlayersPagedAsync(int pageNumber, int pageSize);
    Task<IEnumerable<string>> GetDistinctNationalitiesAsync();
}