public class PlayerCommandService : IPlayerCommandService
{
    private readonly IPlayerRepository _repository;
    public PlayerCommandService(IPlayerRepository repository)
    {
        _repository = repository;
    }

    public Player RecordGoal(string playerName)
    {
        var player = _repository.GetAllPlayers()
                                .FirstOrDefault(p => p.Name.Equals(playerName, StringComparison.OrdinalIgnoreCase));
        if (player == null)
            throw new Exception("Player not found");

        var updated = new Player(player.Name, player.Nationality, player.GoalsScored + 1, player.Age);

        _repository.UpdatePlayer(updated);

        return updated;
    }
}