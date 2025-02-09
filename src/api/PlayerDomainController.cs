using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
[ApiExceptionFilter]
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
        var veterans = await _service.GetVeteranPlayersAsync();
        return Ok(veterans);
    }

    [HttpGet("ordered-by-performance")]
    public async Task<ActionResult<IEnumerable<Player>>> GetPlayersOrderedByPerformanceAsync([FromQuery] bool descending = true)
    {
        var players = await _service.GetPlayersOrderedByPerformanceScoreAsync(descending);
        return Ok(players);
    }
}