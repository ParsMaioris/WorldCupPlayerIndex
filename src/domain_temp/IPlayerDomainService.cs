public interface IPlayerDomainService
{
    Task<IEnumerable<Player>> GetVeteranPlayersAsync();
    Task<IEnumerable<Player>> GetPlayersOrderedByPerformanceScoreAsync(bool descending = true);
}