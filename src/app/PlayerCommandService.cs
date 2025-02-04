public class PlayerCommandService : IPlayerCommandService
{
    private readonly IPlayerRepository _repository;
    public PlayerCommandService(IPlayerRepository repository)
    {
        _repository = repository;
    }

    public async Task<Player> RecordGoalAsync(string playerName)
    {
        var players = await _repository.GetAllPlayersAsync();
        var player = players.FirstOrDefault(p => p.Name.Equals(playerName, StringComparison.OrdinalIgnoreCase));
        if (player == null)
            throw new NotFoundException("Player not found");

        var updated = new Player(player.Name, player.Nationality, player.GoalsScored + 1, player.Age);
        await _repository.UpdatePlayerAsync(updated);
        return updated;
    }

    public async Task<IEnumerable<Player>> RecordGoalsAsync(IEnumerable<string> playerNames)
    {
        var lowerNames = new HashSet<string>(playerNames.Select(n => n.ToLowerInvariant()));
        var players = (await _repository.GetAllPlayersAsync())
            .Where(p => lowerNames.Contains(p.Name.ToLowerInvariant()))
            .ToList();

        var updatedPlayers = players
            .Select(p => new Player(p.Name, p.Nationality, p.GoalsScored + 1, p.Age))
            .ToList();

        foreach (var p in updatedPlayers)
        {
            await _repository.UpdatePlayerAsync(p);
        }
        return updatedPlayers;
    }
}