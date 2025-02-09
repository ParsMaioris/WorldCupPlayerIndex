public class NoPlayersForPerformanceRankingException : DomainException
{
    public NoPlayersForPerformanceRankingException()
        : base("No players are available for performance ranking.")
    {
    }
}