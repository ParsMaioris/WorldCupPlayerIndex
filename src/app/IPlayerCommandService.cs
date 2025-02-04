public interface IPlayerCommandService
{
    Player RecordGoal(string playerName);
    IEnumerable<Player> RecordGoals(IEnumerable<string> playerNames);
}