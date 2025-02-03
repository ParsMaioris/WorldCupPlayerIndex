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
        _repository.GetAllPlayers()
                   .Where(p => p.Nationality.Equals(nationality, StringComparison.OrdinalIgnoreCase));
}