public class EfPlayerRepository : IPlayerRepository
{
    private readonly AppDbContext _context;

    public EfPlayerRepository(AppDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Player> GetAllPlayers()
    {
        return _context.Players.ToList();
    }

    public void UpdatePlayer(Player updatedPlayer)
    {
        var existingPlayer = _context.Players.Find(updatedPlayer.Name);
        if (existingPlayer != null)
        {
            _context.Entry(existingPlayer).CurrentValues.SetValues(updatedPlayer);
            _context.SaveChanges();
        }
    }
}