public interface IPlayerQueryService
{
    IEnumerable<Player> GetPlayersOlderThan(int age);
    IEnumerable<Player> GetPlayersByNationality(string nationality);
    IEnumerable<string> GetPlayerNames();
    int GetTotalGoals();
    IDictionary<string, List<Player>> GetPlayersGroupedByNationality();
    IEnumerable<Player> GetPlayersOrderedByPerformanceScore(bool descending = true);
    bool AnyPlayerReachedGoalThreshold(int goalThreshold);
    Player GetTopPerformer();
    IEnumerable<Player> GetPlayersPaged(int pageNumber, int pageSize);
    IEnumerable<string> GetDistinctNationalities();
}