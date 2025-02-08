public class PlayerCommandService : IPlayerCommandService
{
    private readonly IPlayerRepository _repository;

    public PlayerCommandService(IPlayerRepository repository)
    {
        _repository = repository;
    }

    public async Task<Player> RecordGoalAsync(string playerName)
    {
        if (string.IsNullOrWhiteSpace(playerName))
            throw new InvalidInputException("Player name cannot be empty.");

        var players = await _repository.GetAllPlayersAsync();
        var player = players.FirstOrDefault(p => p.Name.Equals(playerName, StringComparison.OrdinalIgnoreCase));

        if (player == null)
            throw new NotFoundException($"Player '{playerName}' not found.");

        var updated = new Player(player.Name, player.Nationality, player.GoalsScored + 1, player.Age);

        try
        {
            await _repository.UpdatePlayerAsync(updated);
        }
        catch (Exception ex)
        {
            throw new PersistenceException("Failed to update player record.", ex);
        }

        return updated;
    }

    public async Task<IEnumerable<Player>> RecordGoalsAsync(IEnumerable<string> playerNames)
    {
        if (playerNames == null || !playerNames.Any())
            throw new InvalidInputException("Player names list cannot be empty.");

        var lowerNames = new HashSet<string>(playerNames.Select(n => n.ToLowerInvariant()));
        var players = (await _repository.GetAllPlayersAsync())
            .Where(p => lowerNames.Contains(p.Name.ToLowerInvariant()))
            .ToList();

        if (!players.Any())
            throw new NotFoundException("None of the specified players were found.");

        var updatedPlayers = players
            .Select(p => new Player(p.Name, p.Nationality, p.GoalsScored + 1, p.Age))
            .ToList();

        try
        {
            foreach (var p in updatedPlayers)
            {
                await _repository.UpdatePlayerAsync(p);
            }
        }
        catch (Exception ex)
        {
            throw new PersistenceException("Failed to update multiple player records.", ex);
        }

        return updatedPlayers;
    }
}