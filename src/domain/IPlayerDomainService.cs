public interface IPlayerDomainService
{
    Task<IEnumerable<Player>> GetVeteranPlayersAsync();
}