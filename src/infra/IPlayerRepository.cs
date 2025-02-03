public interface IPlayerRepository
{
    IEnumerable<Player> GetAllPlayers();
    void UpdatePlayer(Player updatedPlayer);
}