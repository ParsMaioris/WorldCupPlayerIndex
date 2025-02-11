using Microsoft.EntityFrameworkCore;

public class EfPlayerRepository : IPlayerRepository
{
    private readonly AppDbContext _context;

    public EfPlayerRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Player>> GetAllPlayersAsync()
    {
        return await _context.Players.ToListAsync();
    }

    public async Task UpdatePlayerAsync(Player updatedPlayer)
    {
        try
        {
            var existingPlayer = await _context.Players.FindAsync(updatedPlayer.Name);
            if (existingPlayer == null)
            {
                throw new PersistenceException($"Player '{updatedPlayer.Name}' not found.");
            }

            _context.Entry(existingPlayer).CurrentValues.SetValues(updatedPlayer);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new PersistenceException("An error occurred while updating the player.", ex);
        }
    }
}