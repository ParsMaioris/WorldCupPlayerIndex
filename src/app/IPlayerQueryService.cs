public interface IPlayerQueryService
{
    IEnumerable<Player> GetPlayersOlderThan(int age);
    IEnumerable<Player> GetPlayersByNationality(string nationality);
}