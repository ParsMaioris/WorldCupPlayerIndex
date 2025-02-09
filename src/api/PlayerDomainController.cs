using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PlayerDomainController : ControllerBase
{
    private readonly IPlayerApplicationService _service;

    public PlayerDomainController(IPlayerApplicationService service)
    {
        _service = service;
    }

    [HttpGet("veterans")]
    public async Task<ActionResult<IEnumerable<Player>>> GetVeteranPlayersAsync()
    {
        try
        {
            var veterans = await _service.GetVeteranPlayersAsync();
            return Ok(veterans);
        }
        catch (NoVeteranPlayersFoundException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }

    [HttpGet("ordered-by-performance")]
    public async Task<ActionResult<IEnumerable<Player>>> GetPlayersOrderedByPerformanceAsync([FromQuery] bool descending = true)
    {
        try
        {
            var players = await _service.GetPlayersOrderedByPerformanceScoreAsync(descending);
            return Ok(players);
        }
        catch (NoPlayersForPerformanceRankingException ex)
        {
            return NotFound(new { error = ex.Message });
        }
    }
}