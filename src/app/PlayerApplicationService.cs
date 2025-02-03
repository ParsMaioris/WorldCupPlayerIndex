public class PlayerApplicationService : IPlayerApplicationService
{
    private readonly IPlayerDomainService _playerDomainService;

    public PlayerApplicationService(IPlayerDomainService playerDomainService)
    {
        _playerDomainService = playerDomainService;
    }

    public IEnumerable<Player> GetVeteranPlayers()
    {
        return _playerDomainService.GetVeteranPlayers();
    }
}