public class PlayerDomainService : IPlayerDomainService
{
    private readonly IPlayerRepository _repository;
    public PlayerDomainService(IPlayerRepository repository)
    {
        _repository = repository;
    }

    public IEnumerable<Player> GetVeteranPlayers() =>
        _repository.GetAllPlayers().Where(p => p.IsVeteran());
}