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
        var veterans = players.Where(p => p.IsVeteran()).ToList();
        if (!veterans.Any())
        {
            throw new NoVeteranPlayersFoundException();
        }
        return veterans;
    }
}