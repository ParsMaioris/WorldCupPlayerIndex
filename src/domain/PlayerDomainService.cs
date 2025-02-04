public class PlayerDomainService : IPlayerDomainService
{
    private readonly IPlayerRepository _repository;
    public PlayerDomainService(IPlayerRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Player>> GetVeteranPlayersAsync()
    {
        var players = await _repository.GetAllPlayersAsync();
        return players.Where(p => p.IsVeteran());
    }
}