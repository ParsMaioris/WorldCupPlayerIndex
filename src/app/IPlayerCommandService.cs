public interface IPlayerCommandService
{
    Task<Player> RecordGoalAsync(string playerName);
    Task<IEnumerable<Player>> RecordGoalsAsync(IEnumerable<string> playerNames);
}