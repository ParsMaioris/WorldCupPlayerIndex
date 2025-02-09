public interface IPlayerRepository
{
    Task<List<Player>> GetAllPlayersAsync();
    Task UpdatePlayerAsync(Player updatedPlayer);
}