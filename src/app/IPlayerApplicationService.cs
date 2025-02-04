public interface IPlayerApplicationService
{
    Task<IEnumerable<Player>> GetVeteranPlayersAsync();
}