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
    public IEnumerable<Player> RecordGoals(IEnumerable<string> playerNames)
    {
        var lowerNames = new HashSet<string>(playerNames.Select(n => n.ToLowerInvariant()));
        var players = _repository.GetAllPlayers()
            .Where(p => lowerNames.Contains(p.Name.ToLowerInvariant()))
            .ToList();
        var updatedPlayers = players
            .Select(p => new Player(p.Name, p.Nationality, p.GoalsScored + 1, p.Age))
            .ToList();
        updatedPlayers.ForEach(p => _repository.UpdatePlayer(p));
        return updatedPlayers;
    }
}